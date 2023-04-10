import { Component, OnInit } from '@angular/core';
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
import {Rezervacija} from "../view-models/rezervacija-vm";
import {Usluga} from "../view-models/usluga-vm";
import {RezervacijaAdd} from "../view-models/rezervacija-add-vm";

@Component({
  selector: 'app-moje-rezervacije',
  templateUrl: './moje-rezervacije.component.html',
  styleUrls: ['./moje-rezervacije.component.css']
})
export class MojeRezervacijeComponent implements OnInit {

  rezervacije : Rezervacija[] = null;

  odabranaRezervacija: Rezervacija = null;//brisanje
  //obrisana:boolean=false;
  constructor(private httpKlijent:HttpClient) { }

  ngOnInit(): void {

    this.ucitajRezervacije();
  }

  public ucitajRezervacije() {
    this.httpKlijent.get(MojConfig.adresa_servera + "/Rezervacija/GetAll",MojConfig.http_opcije())
      .subscribe((result : any)=>{
      this.rezervacije = result;
      this.odabranaRezervacija=null;
    });
  }


  prikaziBrisanje(rezervacija: Rezervacija) {
    this.odabranaRezervacija=rezervacija;
  }

  brisanje(rezervacija: Rezervacija) {

    this.httpKlijent.post(MojConfig.adresa_servera + "/Rezervacija/Delete/"+rezervacija.id, rezervacija)
      .subscribe((x:any)=>{
      alert("Odabrana rezervacija je uspješno otkazana");
      this.ucitajRezervacije();
    });

  }
}
