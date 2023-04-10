export class LoginInformacije {
  autentifikacijaToken?:        AutentifikacijaToken|null=null;
  isLogiran:                   boolean=false;
  isPermisijaKorisnik:         boolean=false;
}

export interface AutentifikacijaToken {
  id:                   number;
  vrijednost:           string;
  korisnickiNalogId:    number;
  korisnickiNalog:      KorisnickiNalog;
  vrijemeEvidentiranja: Date;
  ipAdresa:             string;
}

export interface KorisnickiNalog {
  id:                 number;
  korisnickoIme:      string;
  slika_korisnika:    string;
  isAdmin:            boolean;
  isUser:             boolean;
  email:              string;
  isAktiviran:        boolean;
}
