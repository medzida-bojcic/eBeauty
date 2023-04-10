import { Component, OnInit } from '@angular/core';
import {Login} from "../login/view-models/login-vm";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Registracija} from "./view-models/registracija-vm";
import {MojConfig} from "../moj-config";
import {Opstina} from "./view-models/opstina-vm";
import {LoginInformacije} from "../_helpers/login-informacije";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";

@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {

  registracija : Registracija = new Registracija();
  opstine : Opstina[] = null;
  sifra:string;
  prijava : Login = new Login();
  obavjestenje : boolean = false;
  closeModal : boolean = false;
  obavjestenjeNaslov : string = "";
  obavjestenjeSadrzaj : string = "";

  loginInformacije : LoginInformacije = null;
  constructor(private httpKlijent : HttpClient, private router : Router) {
    this.loginInformacije = AutentifikacijaHelper.getLoginInfo();
  }



  ngOnInit(): void {
    this.getAllOpstine();

  }
  private getAllOpstine() {
    this.httpKlijent.get( MojConfig.adresa_servera + "/Opstina/GetAll", MojConfig.http_opcije()).subscribe((result:any)=>{
      this.opstine = result;
    });
  }

  registracijaPodataka() {
    if(this.registracija.password==this.sifra && this.validirajFormu()){


      this.registracija.opstinaId = parseInt(this.registracija.opstinaId.toString());
      this.httpKlijent.post(MojConfig.adresa_servera + "/Korisnik/Add",this.registracija, MojConfig.http_opcije()).subscribe((result:any)=>{

        alert("Uspješno ste se registrovali");
        //this.prikaziObavjestenje("Nova registracija", "Uspješno ste registrovali svoj profil");

        this.prijava.korisnickoIme=this.registracija.username;
        this.prijava.lozinka=this.registracija.password;

        this.httpKlijent.post(MojConfig.adresa_servera+'/Autentifikacija/Login',this.prijava).subscribe((response : any)=>{

            localStorage.setItem("autentifikacija-token", JSON.stringify(response));
            this.router.navigateByUrl("/home");


          }
        )

      });}

    else this.prikaziObavjestenje("Neadekvatno ispunjena forma za registraciju", "Molimo ispunite sva obavezna polja ili ispravno unesite lozinku, pa ponovo pokušajte");



  }
  private prikaziObavjestenje(naslov : string, sadrzaj : string) {
    this.obavjestenje = true;
    this.closeModal = false;
    this.obavjestenjeNaslov = naslov;
    this.obavjestenjeSadrzaj = sadrzaj;
  }

  private validirajFormu() {
    var osnovneInformacije : boolean = this.registracija.username != null && this.registracija.username?.length > 0
      && this.registracija.password != null && this.registracija.password?.length > 0
      && this.registracija.ime != null && this.registracija.ime?.length > 0
      && this.registracija.prezime != null && this.registracija.prezime?.length > 0
      && this.registracija.email != null && this.registracija.email?.length > 0;
    var dodatneInformacije : boolean = true;
    if (this.loginInformacije.isPermisijaKorisnik)
      dodatneInformacije = this.registracija.adresa != null && this.registracija.adresa?.length > 0
        && this.registracija.brojTelefona != null && this.registracija.brojTelefona?.length > 0
        && this.registracija.opstinaId != null;

    return osnovneInformacije && dodatneInformacije;
  }
  provjeriPolje(polje: any) {
    if (polje.invalid && (polje.dirty || polje.touched)){
      if (polje.errors?.['required']){
        return 'Niste popunili ovo polje!';
      }
      else {
        return '';
      }
    }
    return 'Obavezno polje za unos';
  }
  animirajObavjestenje() {
    return this.closeModal == true? 'animate__animated animate__bounceOut' : 'animate__animated animate__bounceIn';
  }

  zatvoriModalObavjestenje(){
    this.closeModal = true;
    this.animirajObavjestenje();
    setTimeout(() => {
      this.obavjestenje = false;
    },1000);
  }

}
