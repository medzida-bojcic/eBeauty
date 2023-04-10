import { Component } from '@angular/core';
import {NavigationStart, Router} from "@angular/router";
import {AutentifikacijaHelper} from "./_helpers/autentifikacija-helper";
import {MojConfig} from "./moj-config";
import {LoginInformacije} from "./_helpers/login-informacije";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BeautyShopAngular';
  loginInformacije: LoginInformacije = new LoginInformacije();
  trenutnaSelekcija: string = "Home";
  closeModal: boolean = false;

  constructor(private router : Router, private httpKlijent: HttpClient) {
    router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.loginInformacije = this.loginInfo();
      }
    });
  }

    loginInfo(): LoginInformacije
    {
      return AutentifikacijaHelper.getLoginInfo();
    }

  clicked(naziv : string){
    this.trenutnaSelekcija = naziv;
  }

  animirajNotifikaciju() {
    return this.closeModal == true? 'animate__animated animate__bounceOutUp' : 'animate__animated animate__bounceInDown';
  }

  preusmjeri() {
    if (this.trenutnaSelekcija!='Home') this.router.navigate(['/home']);
    setTimeout(()=>{
      this.router.navigate(['/home'], {fragment:'kontakt'});
    },500);
  }
  odjava() {
    this.httpKlijent.post(MojConfig.adresa_servera + "/Autentifikacija/Logout", null, MojConfig.http_opcije())
      .subscribe((x: any) => {
      });
    AutentifikacijaHelper.ocistiMemoriju();
  }
}
