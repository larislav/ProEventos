import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, RequiredValidator, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  form!: FormGroup;
  get f(): any {
    return this.form.controls;

  }
  constructor(private formbuilder: FormBuilder) { }

  ngOnInit(): void {
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

}
