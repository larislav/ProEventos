import { Component, OnInit, TemplateRef } from '@angular/core';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public exibirImagem = true;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId = 0;
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
              private spinner: NgxSpinnerService,
              private router: Router) { }
  //evento.service está sendo injetado no construtor
  //ngOnInit(): metodo que é camado antes de inicializar a aplicação,
  //antes do html ser interpretado
  public ngOnInit(): void {
    /* spinner starts on init */
    this.spinner.show();
    this.carregarEventos();

    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    // }, 2000);
  }

  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL !== '')
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : 'assets/semImagem.png';
  }

  public carregarEventos(): void{
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

  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next:(result: any) =>{
        if(result.message === 'Deletado'){
          this.toastr.success('O Evento foi deletado com sucesso', 'Deletado!');
          this.carregarEventos();
        }
      },
      error:(error: any) =>{
        this.toastr.error(`Erro ao deletar o evento ${this.eventoId}`, 'Erro!');
      }
    }).add(() => this.spinner.hide());

  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
