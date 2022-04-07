import { Component, OnInit, TemplateRef } from '@angular/core';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public exibirImagem = true;
  public eventos: Evento[] = [];
  public eventoId = 0;
  public widthImg = 150;
  public marginImg = 2;
  public pagination = {} as Pagination;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evnt: any) : void{
    if(this.termoBuscaChanged.observers.length == 0){

      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.eventoService.getEvento(this.pagination.currentPage,
            this.pagination.itemsPerPage,
            evnt.value).subscribe({
              next:(paginatedResult: PaginatedResult<Evento[]>)=>{
                this.eventos = paginatedResult.result;
                this.pagination = this.pagination;
              },
              error:(error: any)=>{
                this.spinner.hide();
                this.toastr.error('Erro ao carregar os eventos', "Erro!");
              }
            }).add(()=> this.spinner.hide());
        }
      )
    }
    this.termoBuscaChanged.next(evnt.value); // vai entrar aqui caso não tenha nada no termoBuscaChanged
                                  //e vai alterar o termoBuscaChanged para ter pelo menos 1 item
                                  // e qnd ele tiver 1 item pq vc deu o next dele, ele vai lá no pipe
                                  //e executa de 1 em 1 segundo
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
    this.pagination = {currentPage:1, itemsPerPage: 1, totalItems: 2} as Pagination;
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
    this.spinner.show();

    this.eventoService.getEvento(this.pagination.currentPage,
                                this.pagination.itemsPerPage).subscribe({
      next: (response: PaginatedResult<Evento[]>) => {
        this.eventos = response.result;
        this.pagination = response.pagination;
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

  public pageChanged(event): void{
    this.pagination.currentPage = event.page;

    this.carregarEventos();
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
