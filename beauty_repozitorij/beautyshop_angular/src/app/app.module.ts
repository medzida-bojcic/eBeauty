import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule} from "@angular/forms"
import { AppComponent } from './app.component';
import { HttpClientModule} from "@angular/common/http";
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegistracijaComponent } from './registracija/registracija.component';
import { MojiPodaciComponent } from './moji-podaci/moji-podaci.component';
import { HomeComponent } from './home/home.component';
import { NarudzbaComponent } from './narudzba/narudzba.component';
import { ProizvodiComponent } from './proizvodi/proizvodi.component';
import {NarudzbeComponent} from './narudzbe/narudzbe.component';
import {RezervacijaComponent} from './rezervacija/rezervacija.component';
import {MojeRezervacijeComponent} from './rezervacija/moje-rezervacije/moje-rezervacije.component'
import {TwoFOtkljucajComponent} from "./two-f-otkljucaj/two-f-otkljucaj.component";
import {AutorizacijaLoginProvjera} from "./_guards/autorizacija-login-provjera.service";
import { NotFoundComponent } from './not-found/not-found.component';
import { AppRoutingModule } from './app-routing.module';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistracijaComponent,
    HomeComponent,
    NotFoundComponent,
    MojiPodaciComponent,
    NarudzbaComponent,
    ProizvodiComponent,
    NarudzbeComponent,
    RezervacijaComponent,
    MojeRezervacijeComponent,
    TwoFOtkljucajComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      {path: 'two-f-otkljucaj', component: TwoFOtkljucajComponent},
      {path: 'login', component: LoginComponent},
      {path: 'registracija', component: RegistracijaComponent},
      {path: 'home', component: HomeComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'narudzba', component: NarudzbaComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'moji-podaci', component: MojiPodaciComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'proizvodi', component: ProizvodiComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'narudzbe', component: NarudzbeComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'rezervacija', component: RezervacijaComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: 'moje-rezervacije', component: MojeRezervacijeComponent, canActivate: [AutorizacijaLoginProvjera]},
      {path: '**', component: NotFoundComponent},
    ]),
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
  ],
  providers: [
    AutorizacijaLoginProvjera,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
