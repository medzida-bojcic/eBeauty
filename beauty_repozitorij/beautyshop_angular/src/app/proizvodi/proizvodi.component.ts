import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProizvodiStavka} from "./view-models/proizvodi-stavka-vm";
import {StavkaNarudzbe} from '../narudzbe/view-models/stavka-narudzbe-vm';
import {ProizvodiStavkaKorisnik} from './view-models/proizvodi-stavka-korisnik-vm'
import {MojConfig} from "../moj-config";
import {ProizvodiKategorija} from "./view-models/proizvodi-kategorija-vm";
import {Router} from "@angular/router";
import {LoginInformacije} from "../_helpers/login-informacije";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";


@Component({
  selector: 'app-proizvodi',
  templateUrl: './proizvodi.component.html',
  styleUrls: ['./proizvodi.component.css']
})
export class ProizvodiComponent implements OnInit {
  proizvodiStavka : ProizvodiStavka[] = null;
  proizvodKategorija : ProizvodiKategorija[] = null;
  proizvodStavkaKorisnik: ProizvodiStavkaKorisnik[] = null;
  novaStavkaNarudzbe : StavkaNarudzbe=new StavkaNarudzbe();
  id : number = null;
  loginInformacije : LoginInformacije = null;
  odabranaStavka: ProizvodiStavka = null;
  trenutnaKategorija: string = "Tečni puder";
  obavjestenje : boolean = false;
  closeModal : boolean = false;
  obavjestenjeNaslov : string = "";
  obavjestenjeSadrzaj : string = "";


  constructor(private httpKlijent : HttpClient, private router : Router) {
    this.loginInformacije = AutentifikacijaHelper.getLoginInfo();

  }

  ngOnInit(): void {
    this.getKategorija();
    this.ucitajProizvodiStavke();
  }
  public ucitajProizvodiStavke(nazivKategorije : string = "Tečni puderi") {
    this.trenutnaKategorija = nazivKategorije;
    this.httpKlijent.get(MojConfig.adresa_servera + "/Proizvod/GetAllPaged?nazivKategorije="+nazivKategorije, MojConfig.http_opcije())
      .subscribe((result : any)=>{
        this.proizvodiStavka = result;
        this.odabranaStavka=null;
      })
  }


  createRange(ocjena: number) {
    let velicina = Math.round(ocjena);
    return new Array(velicina);
  }

  private getKategorija() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Kategorija/GetAll", MojConfig.http_opcije())
      .subscribe((result : any)=>{
        this.proizvodKategorija = result;
      })
  }


  dodajUKorpu(stavka : ProizvodiStavkaKorisnik) {
    this.novaStavkaNarudzbe.proizvodId = stavka.id;
    this.httpKlijent.post(MojConfig.adresa_servera+"/Narudzba/AddStavka",this.novaStavkaNarudzbe,
      MojConfig.http_opcije())
      .subscribe((response : any)=>{
        document.getElementById('kolicina').innerHTML = response;
      });
  }

  animirajObavjestenje() {
    return this.closeModal == true? 'animate__animated animate__bounceOut' : 'animate__animated animate__bounceIn';
  }

  zatvoriModalObavjestenje(){
    this.closeModal = true;
    this.animirajObavjestenje();
    setTimeout(()=>{
      this.obavjestenje = false;
    },500);
  }

  zatvoriModal(){
    this.closeModal = true;
    this.animirajObavjestenje();

    setTimeout(()=>{
      this.odabranaStavka = null;
    },1000);
  }
}
