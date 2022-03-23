import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, RequiredValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  eventoId: number;
  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar = 'post';
  loteAtual = {id:0, nome: '', indice: 0};
  imagemURL = 'assets/upload.png';
  file: File;

  get modoEditar(): boolean{
    return this.estadoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  //como quero utilizar como propriedade, apenas seto como get
  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false

    };
  }

  public retornaTituloLote(nome: string):string{
    return nome === null || nome == ''
                ?  'Nome do lote'
                : nome
  }

  public mudarValorData(value: Date, indice: number, campo: string): void{
    this.lotes.value[indice][campo] = value;
  }

  constructor(private formbuilder: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService)
    {
      this.localeService.use('pt-br');
    }

    public carregarEvento(): void{
      this.eventoId = +this.activatedRoute.snapshot.paramMap.get('id');
      if(this.eventoId != null && this.eventoId !== 0){
        this.spinner.show();
        this.estadoSalvar = 'put';

        this.eventoService.getEventoPorId(this.eventoId).subscribe({
          next: (evento: Evento) => {
            this.evento = {... evento}; // usando spread invés de simplesmente setar
            //this.evento = evento para não apontar para memóriado obj retornado,
            //e sim criar um novo objeto com os valores copiados
            this.form.patchValue(this.evento);
            if(this.evento.imagemURL !== ''){
              this.imagemURL = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
            }
            this.carregarLotes();
          },
          error: (error: any) => {
            this.toastr.error('Erro ao carregar o evento.', 'Erro!');
          }
        }).add(() => this.spinner.hide());
      }
    }

    public carregarLotes(): void {
      this.loteService.getLotesByEventoId(this.eventoId).subscribe({
        next: (lotesRetorno: Lote[]) => {
          lotesRetorno.forEach(lote => {
            console.log(lote);
            this.lotes.push(this.criarLote(lote));
          })
        },
        error:(error: any)=>{
          this.toastr.error('Erro ao tentar carregar lotes', 'Erro');
        }
      }).add(() => this.spinner.hide());
    }

    ngOnInit(): void {
      this.carregarEvento();
      this.validation();
    }

    public validation(): void{
      this.form = this.formbuilder.group({
        tema:['',[
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50)
        ]],
        local: ['', Validators.required],
        dataEvento:['',Validators.required],
        qtdPessoas:['',[
          Validators.required,
          Validators.max(120000)
        ]],
        imagemURL:[''],
        telefone:['',Validators.required],
        email:['',[
          Validators.required,
          Validators.email
        ]],
        lotes: this.formbuilder.array([])
      });
    }

    adicionarLote(): void{
      this.lotes.push(this.criarLote({id: 0} as Lote));
    }

    criarLote(lote: Lote) : FormGroup{
      return this.formbuilder.group({
        id:[lote.id],
        nome:[lote.nome, Validators.required],
        quantidade:[lote.quantidade, Validators.required],
        preco:[lote.preco, Validators.required],
        dataInicio:[lote.datainicio],
        dataFim:[lote.datafim]
      });
    }

    onSubmit():void{
      if(this.form.invalid){
        return;
      }
    }

    public resetForm(event: any): void{
      event.preventDefault();
      this.form.reset();
    }

    public cssValidator(campoForm: FormControl | AbstractControl): any{
      return {'is-invalid' : campoForm.errors && campoForm.touched};
    }

    public salvarEvento(): void{
      if(this.form.valid){
        this.spinner.show();
        this.evento = (this.estadoSalvar == 'post')
          ? {... this.form.value}
          : {id:this.evento.id, ... this.form.value};

        this.eventoService[this.estadoSalvar](this.evento).subscribe({
          next: (eventoRetorno: Evento) => {
            this.toastr.success('O Evento salvo com sucesso', 'Sucesso!');
            this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
          },
          error: (error:any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao salvar o evento', 'Erro!');
          },
          complete: () => {
            this.spinner.hide();
          }
        });

      }
    }


    public salvarLote(): void{
      this.spinner.show();
      if(this.form.controls.lotes.valid){
        this.loteService.saveLote(this.eventoId, this.form.value.lotes)
        .subscribe({
          next:() => {
            this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
            //this.lotes.reset();
          },
          error:(error: any) => {
            this.toastr.error('Erro ao tentar salvar lotes.', 'Erro');
            console.error(error);
          }
        }).add(()=> this.spinner.hide());

        this.spinner.hide();
      }
    }

    public removerLote(template: TemplateRef<any>,
                        indice: number): void{

      this.loteAtual.id =this.lotes.get(indice + '.id').value;
      this.loteAtual.nome =this.lotes.get(indice + '.nome').value;
      this.loteAtual.indice = indice;

      this.modalRef = this.modalService.show(template, {class:'modal-sm'})
      //this.lotes.removeAt(indice);
    }

    confirmDeleteLote():void{
      this.modalRef.hide();
      this.spinner.show();
      this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe({
        next: () => {
          this.toastr.success('Lote deletado com sucesso', 'Sucesso');
          this.lotes.removeAt(this.loteAtual.indice);
        },
        error: (error: any) => {
          this.toastr.error(`Erro ao tentar deletar o Lote ${this.loteAtual.id}`, 'Erro');
        }
      }).add(()=> this.spinner.hide());
    }
    declineDeleteLote():void{
      this.modalRef.hide();
    }

    onFileChange(ev: any): void{
      const reader = new FileReader();
      reader.onload = (event: any) => this.imagemURL = event.target.result;
      this.file = ev.target.files;
      reader.readAsDataURL(this.file[0]);
      this.uploadImagem();
    }

    uploadImagem(): void{
      this.spinner.show();
      this.eventoService.postUpload(this.eventoId, this.file).subscribe({
        next:() => {
          //this.carregarEvento();
         this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
          this.toastr.success('Imagem atualizada com sucesso', 'Sucesso!');
        },
        error:(error: any) => {
          this.toastr.error('Erro no upload da imagem', 'Erro!');
          console.log(error);
        }
      }).add(() => this.spinner.hide());
    }

  }
