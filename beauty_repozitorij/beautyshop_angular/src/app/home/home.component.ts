import {Component, OnInit} from '@angular/core';
import {LoginInformacije} from "../_helpers/login-informacije";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";

@Component({
  selector:'app-home',
  templateUrl:'./home.component.html',
  styleUrls:['./home.component.css']

})

export class HomeComponent implements OnInit{
  constructor() {

  }
ngOnInit() {
}

loginInfo():LoginInformacije{
    return AutentifikacijaHelper.getLoginInfo();
}
}
