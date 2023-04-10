import { Component, OnInit } from '@angular/core';
import {Usluga} from "./view-models/usluga-vm";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {RezervacijaAdd} from "./view-models/rezervacija-add-vm";

@Component({
  selector: 'app-rezervacija',
  templateUrl: './rezervacija.component.html',
  styleUrls: ['./rezervacija.component.css']
})
export class RezervacijaComponent implements OnInit {
  public usluge : Usluga[] = null;
  rezervacija:RezervacijaAdd= new RezervacijaAdd();

  obavjestenje : boolean = false;
  closeModal : boolean = false;
  obavjestenjeNaslov : string = "";
  obavjestenjeSadrzaj : string = "";
  constructor(private httpKlijent:HttpClient) { }

  ngOnInit(): void {
    this.getAllUsluge();
    //this.rezervacija.UslugaId = -1;
  }

  private getAllUsluge() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Usluga/GetAll", MojConfig.http_opcije())
      .subscribe((result:any)=>{
      this.usluge = result;

    });
  }

  posaljiPodatke() {
    if(this.validirajFormu()) {
      this.httpKlijent.post(MojConfig.adresa_servera + "/Rezervacija/Add", this.rezervacija, MojConfig.http_opcije())
        .subscribe((result: any) => {

        this.prikaziObavjestenje("Nova rezervacija", "Uspješno ste dodali novu rezervaciju");
        this.ocistiFormu();

      });
    }
    else {
      this.prikaziObavjestenje("Neadekvatno ispunjena forma za dodavanje nove meni stavke", "Molimo ispunite sva obavezna polja, pa ponovo pokušajte");
    }

  }
  ocistiFormu(){
    this.rezervacija.datumRezervacije = null;
    this.rezervacija.uslugaId = -1;

  }
  validirajFormu() : boolean{


    return this.rezervacija.datumRezervacije != null && this.rezervacija.datumRezervacije?.length > 0
      && this.rezervacija.uslugaId != -1;
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

  private prikaziObavjestenje(naslov : string, sadrzaj : string) {
    this.obavjestenje = true;
    this.closeModal = false;
    this.obavjestenjeNaslov = naslov;
    this.obavjestenjeSadrzaj = sadrzaj;
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
