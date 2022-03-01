
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/evento.service';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
  //providers: [EventoService] // forma de injetar o service
})
export class EventosComponent implements OnInit {
  modalRef?: BsModalRef;
  public exibirImagem = true;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImg = 150;
  public marginImg = 2;
  private _filtroLista = '';

  public get filtroLista(){
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  public filtrarEventos(filtrarPor: string) : Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento : any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(private eventoService: EventoService,
              private modalService: BsModalService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }
  //evento.service está sendo injetado no construtor
  //ngOnInit(): metodo que é camado antes de inicializar a aplicação,
  //antes do html ser interpretado
  public ngOnInit(): void {
    /* spinner starts on init */
    this.spinner.show();
    this.getEventos();

    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    // }, 2000);
  }

  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public getEventos(): void{
    this.eventoService.getEvento().subscribe({
      next: (response: Evento[]) => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro!');
      },
      complete: () => this.spinner.hide()
    });
    //requisitar do meu metodo get do http e vou me inscrever nesse observable
    //que me retorna 2 itens principalmente: o response, e o complete
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O Evento foi deletado com sucesso', 'Deletado');
  }

  decline(): void {
    this.modalRef?.hide();
  }


}
