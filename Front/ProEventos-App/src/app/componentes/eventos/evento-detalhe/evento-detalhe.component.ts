import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, RequiredValidator, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  evento= {} as Evento;
  form!: FormGroup;
  estadoSalvar = 'post';
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

  constructor(private formbuilder: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService)
    {
      this.localeService.use('pt-br');
    }

    public carregarEvento(): void{
      const eventoIdParam = this.router.snapshot.paramMap.get('id');
      if(eventoIdParam != null){
        this.spinner.show();
        this.estadoSalvar = 'put';
        this.eventoService.getEventoPorId(+eventoIdParam).subscribe({
          next: (evento: Evento) => {
            this.evento = {... evento}; // usando spread invés de simplesmente setar
            //this.evento = evento para não apontar para memóriado obj retornado,
            //e sim criar um novo objeto com os valores copiados
            this.form.patchValue(this.evento);
          },
          error: (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao carregar o evento.', 'Erro!');
          },
          complete: () => {
            this.spinner.hide();
          }
        })
      }
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
        imagemURL:['',Validators.required],
        telefone:['',Validators.required],
        email:['',[
          Validators.required,
          Validators.email
        ]]
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

    public cssValidator(campoForm: FormControl): any{
      return {'is-invalid' : campoForm.errors && campoForm.touched};
    }

    public salvarAlteracao(): void{
      this.spinner.show();

      if(this.form.valid){

        this.evento = (this.estadoSalvar == 'post')
          ? {... this.form.value}
          : {id:this.evento.id, ... this.form.value};

        this.eventoService[this.estadoSalvar](this.evento).subscribe({
          next: () => {
            this.toastr.success('O Evento salvo com sucesso', 'Sucesso!');
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

  }
