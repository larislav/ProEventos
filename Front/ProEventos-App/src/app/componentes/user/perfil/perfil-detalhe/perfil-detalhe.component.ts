import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  @Output () changeFormValue = new EventEmitter();

  form!: FormGroup;

  constructor(private formbuilder: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService,
    public palestranteService: PalestranteService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  private verificaForm(): void{
    //valueChanges retorna um Observable, isso quer dizer que posso usar um subscribe
    this.form.valueChanges.subscribe({
      next:()=>{
        this.changeFormValue.emit({... this.form.value})//toda vez que form chamado esse emit, vai emitir um alerta
      },                  //vai chamar o changeFormValue. O changeFormValue dentro do perfil.component vai chamar
                          //a função setFormValue($event) que vai passar todas a informações do formulário
                          //através do $event para o setFormValue no typescript do perfil.component
      error:()=>{}
    })
  }

  get f():any{
    return this.form.controls;
  }

  onSubmit():void{
    this.atualizarUsuario();
 }

  private carregarUsuario(): void{
    this.spinner.show();
    this.accountService.getUser().subscribe({
      next:(userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado', 'Sucesso');
      },
      error:(error) => {
        console.error(error);
        this.toaster.error('Usuário não carregado', 'Erro');
        this.router.navigate(['/dashboard']);
      }
    }
    ).add(() => this.spinner.hide());

  }

  private validation () : void{
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.formbuilder.group({
      userName: [''],
      primeiroNome: ['',[Validators.required]],
      ultimoNome: ['',[Validators.required]],
      titulo: ['NaoInformado',[Validators.required]],
      email: ['',
      [
        Validators.required,
        Validators.email
      ]],
      phoneNumber: ['',[Validators.required]],
      funcao: ['NaoInformado',[Validators.required]],
      descricao: ['',
      [
        Validators.required,
        Validators.maxLength(500)
      ]],
      password: ['',
      [
        Validators.nullValidator,
        Validators.minLength(4)
      ]],
      confirmePassword: ['',[Validators.nullValidator]],
      imagemURL: ['']

    }, formOptions)
  }



 public atualizarUsuario(){
  this.userUpdate = { ...this.form.value};
  this.spinner.show();

  if(this.f.funcao.value == 'Palestrante'){
    //chamar função de registrar na tabela de palestrante
    this.palestranteService.post().subscribe({
      next:()=>{
        this.toaster.success('Função palestrante ativada!', 'Sucesso');
      },
      error:(error:any)=>{
        this.toaster.error('A função palestrante não pôde ser ativada', 'Error');
        console.error(error);
      }
    })

  }
  this.accountService.updateUser(this.userUpdate).subscribe(
    {
      next:() => {this.toaster.success('Usuário atualizado!', 'Sucesso')},
      error:(error) => {
        this.toaster.error(error.error);
        console.error(error);
      }
    }
  ).add(() => this.spinner.hide())
}

public resetForm(event: any): void{
  event.preventDefault();
  this.form.reset();
}

}
