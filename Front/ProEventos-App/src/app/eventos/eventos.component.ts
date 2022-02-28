import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  exibirImagem = true;
  public eventos: any = [];
  public eventosFiltrados: any = [];
  widthImg: number = 150;
  marginImg: number = 2;
  private _filtroLista: string = '';

  public get filtroLista(){
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  filtrarEventos(filtrarPor: string) : any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento : any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(private http: HttpClient) { }

  //ngOnInit(): metodo que é camado antes de inicializar a aplicação,
  //antes do html ser interpretado
  ngOnInit(): void {
    this.getEventos()
  }

  alterarImagem(){
    this.exibirImagem = !this.exibirImagem;
  }

  public getEventos(): void{
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos
      },
      error => console.log(error)
    );
    //requisitar do meu metodo get do http e vou me inscrever nesse observable
    //que me retorna 2 itens principalmente: o response, e o complete

  }

}
