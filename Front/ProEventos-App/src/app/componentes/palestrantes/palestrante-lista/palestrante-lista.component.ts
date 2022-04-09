import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {
  public palestrantes: Palestrante[] = [];
  public eventoId = 0;
  public pagination = {} as Pagination;

  constructor(private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public ngOnInit() {
    this.pagination = {currentPage:1, itemsPerPage: 1, totalItems: 2} as Pagination;
    this.carregarPalestrantes();
  }

  public getImagemURL(imagemName: string): string{
    if(imagemName)
      return environment.apiURL + `resourcers/perfil/${imagemName}`;
    else
      return  './assets/semImagem.png';
  }


  public carregarPalestrantes(): void{
    this.spinner.show();

    this.palestranteService.getPalestrantes(this.pagination.currentPage,
                                this.pagination.itemsPerPage).subscribe({
      next: (response: PaginatedResult<Palestrante[]>) => {
        this.palestrantes = response.result;
        this.pagination = response.pagination;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os palestrantes', 'Erro!');
      },
      complete: () => this.spinner.hide()
    });
    //requisitar do meu metodo get do http e vou me inscrever nesse observable
    //que me retorna 2 itens principalmente: o response, e o complete
  }

  public filtrarPalestrante(evnt: any) : void{
    if(this.termoBuscaChanged.observers.length == 0){

      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.palestranteService.getPalestrantes(this.pagination.currentPage,
            this.pagination.itemsPerPage,
            evnt.value).subscribe({
              next:(paginatedResult: PaginatedResult<Palestrante[]>)=>{
                this.palestrantes = paginatedResult.result;
                this.pagination = this.pagination;
              },
              error:(error: any)=>{
                this.spinner.hide();
                this.toastr.error('Erro ao carregar os palestrantes', "Erro!");
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

}
