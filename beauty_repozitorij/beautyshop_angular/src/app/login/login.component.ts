import { ComponentFixture, TestBed } from '@angular/core/testing';


import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Login} from "./view-models/login-vm";
import {Router} from "@angular/router";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";
import {LoginInformacije} from "../_helpers/login-informacije";
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  prijava : Login = new Login();
  zapamtiMe : boolean;
  fieldText: boolean;
  validiranoKorisnickoIme: boolean = false;
  validiranaLozinka : boolean = false;
  obavjestenje : boolean = false;
  closeModal : boolean = false;
  obavjestenjeNaslov : string = "";
  obavjestenjeSadrzaj : string = "";

  constructor(private httpKlijent : HttpClient, private router : Router) {
  }

  ngOnInit(): void {
  }

  posaljiPodatke() {
    if (this.validiranoKorisnickoIme && this.validiranaLozinka) {
      this.httpKlijent.post(MojConfig.adresa_servera + '/Autentifikacija/Login', this.prijava)
        .subscribe((response: any) => {
          if (response.isLogiran) {
            response.isPermisijaGost = false;
            AutentifikacijaHelper.setLoginInfo(response, this.zapamtiMe);
            this.router.navigateByUrl("/home");
            if(response.autentifikacijaToken?.korisnickiNalog.isUser)
              this.router.navigateByUrl("/two-f-otkljucaj");
          } else {
            AutentifikacijaHelper.setLoginInfo(null);
            this.prikaziObavjestenje("Pogrešno uneseni podaci za prijavu", "Neispravno korisničko ime / lozinka");
          }
        }
      );
    }
    else this.prikaziObavjestenje("Neadekvatno ispunjena forma za prijavu", "Molimo ispunite sva obavezna polja, pa ponovo pokušajte");
  }

  prikaziRegistraciju() {
    this.router.navigate(['/registracija']);
  }

  prikaziSakrij() {
    this.fieldText = !this.fieldText;
  }

  provjeriKorisnickoIme(polje : any) {
    if (polje.invalid && (polje.dirty || polje.touched)){
      if (polje.errors?.['required']){
        this.validiranoKorisnickoIme = false;
        return 'Niste popunili ovo polje!';
      }
      else {
        this.validiranoKorisnickoIme = true;
        return '';
      }
    }
    if (this.prijava.korisnickoIme != null && this.prijava.korisnickoIme.length > 0) this.validiranoKorisnickoIme = true;
    return 'Obavezno polje za unos';
  }

  provjeriLozinku(polje : any) {
    if (polje.invalid && (polje.dirty || polje.touched)){
      if (polje.errors?.['required']){
        this.validiranaLozinka = false;
        return 'Niste popunili ovo polje!';
      }
      else {
        this.validiranaLozinka = true;
        return '';
      }
    }
    if (this.prijava.lozinka != null && this.prijava.lozinka.length > 0) this.validiranaLozinka = true;
    return 'Obavezno polje za unos';
  }

  animirajObavjestenje() {
    return this.closeModal == true? 'animate__animated animate__bounceOutUp' : 'animate__animated animate__bounceInDown';
  }

  zatvoriModalObavjestenje(){
    this.closeModal = true;
    this.animirajObavjestenje();
    setTimeout(() => {
      this.obavjestenje = false;
    },500);
  }

  private prikaziObavjestenje(naslov : string, sadrzaj : string) {
    this.obavjestenje = true;
    this.closeModal = false;
    this.obavjestenjeNaslov = naslov;
    this.obavjestenjeSadrzaj = sadrzaj;
  }
}
