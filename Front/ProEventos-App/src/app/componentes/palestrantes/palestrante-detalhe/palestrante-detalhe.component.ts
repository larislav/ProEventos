import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {
  public form: FormGroup;
  public situacaoDoForm = '';
  public corDaDescricao = '';

  constructor(private formBuilder: FormBuilder,
              private palestranteService: PalestranteService,
              private toaster: ToastrService,
              private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.verificaForm();
    this.carregarPalestrante();
  }

  public get f(): any{
    return this.form.controls;
  }

  private carregarPalestrante(): void {
    this.spinner.show();

    this.palestranteService.getPalestrante().subscribe({
      next:(palestrante: Palestrante) => {
        this.form.patchValue(palestrante);
      },
      error:(error: any) => {
        this.toaster.error(error.message, 'Erro');

      }
    })
  }

  private verificaForm(): void {
  // this.form.valueChanges.pipe(
  //   map(() => {
  //       this.situacaoDoForm = 'Minicurrículo está sendo atualizado'
  //       this.corDaDescricao = 'text-warning';
  //     }),
  //     debounceTime(1000), // debouncetime vai segurar o estado do observable até que pare de ser atualizado,
  //     tap(() => this.spinner.show()) //e aguarda o tempo especificado para executar a ação
  //   ).subscribe({
  //     next:() => {
  //       this.palestranteService.put({... this.form.value}).subscribe({
  //         next:() => {
  //           this.situacaoDoForm = 'Minicurrículo foi atualizado';
  //           this.corDaDescricao = 'text-muted';

  //           setTimeout(() => {
  //             this.situacaoDoForm = 'Minicurrículo carregado';
  //           this.corDaDescricao = 'text-success';
  //           }, 2000)
  //         },
  //         error:(error: any) => {
  //           this.toaster.error('Erro ao tentar atualizar palestrante', 'Erro');
  //         }
  //       }).add(() => this.spinner.hide())
  //     },
  //     error:() => {}
  //   })
  }

  private validation(): void {
    this.form = this.formBuilder.group({
      minicurriculo: ['']
    })
  }

}
