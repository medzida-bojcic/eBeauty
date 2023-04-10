import {NarudzbaStavkaVm} from './narudzba-stavka-vm';

export class NarudzbaVm{
  id:number;
  cijena:number;
  datumNarudzbe:string;
  statusNarudzbe:string;
  stavke:NarudzbaStavkaVm[]
}
