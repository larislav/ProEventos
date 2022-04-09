import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { RedeSocial } from '@app/models/RedeSocial';
import { RedeSocialService } from '@app/services/redeSocial.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redesSociais',
  templateUrl: './redesSociais.component.html',
  styleUrls: ['./redesSociais.component.scss']
})
export class RedesSociaisComponent implements OnInit {
  modalRef: BsModalRef;
  @Input() eventoId = 0;
  public formRS: FormGroup;
  public redeSocialAtual = {id: 0, nome: '', indice: 0};

  public get redesSociais(): FormArray {
    return this.formRS.get('redesSociais') as FormArray;
  }

  constructor(private formBuilder: FormBuilder,
              private modalService: BsModalService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private redeSocialService: RedeSocialService) { }

  ngOnInit() {
    if(this.eventoId === 0){
      this.carregarRedesSociais(this.eventoId);
    }
    this.validation();
  }

  private carregarRedesSociais(id: number = 0): void{
    let origem = 'palestrante';
    if(this.eventoId !== 0) origem = 'evento';
    this.spinner.show();

    this.redeSocialService.getRedesSociais(origem, id).subscribe({
      next:(redesSociaisRetorno: RedeSocial[]) => {
        redesSociaisRetorno.forEach((redeSocial) => {
          this.redesSociais.push(this.criarRedeSocial(redeSocial));
        });
      },
      error:(error: any) => {
        this.toastr.error('Erro ao carregar redes sociais', 'Erro');
        console.error(error);
      }
    }).add(() => this.spinner.hide());
  }

  public validation(): void{
    this.formRS = this.formBuilder.group({
      redesSociais: this.formBuilder
    })
  }

  adicionarRedeSocial(): void{
    this.redesSociais.push(this.criarRedeSocial({id: 0} as RedeSocial));
  }

  criarRedeSocial(redeSocial: RedeSocial) : FormGroup{
    return this.formBuilder.group({
      id:[redeSocial.id],
      nome:[redeSocial.nome, Validators.required],
      url:[redeSocial.url, Validators.required]
    });
  }

  public retornaTitulo(nome: string):string{
    return nome === null || nome == ''
                ?  'Rede Social'
                : nome
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any{
    return {'is-invalid' : campoForm.errors && campoForm.touched};
  }

  public salvarRedeSocial(): void{
    let origem = 'palestrante';
    if(this.eventoId !== 0)
      origem = 'evento';

    if(this.formRS.controls.lotes.valid){
      this.spinner.show();
      this.redeSocialService.saveRedesSociais(origem, this.eventoId, this.formRS.value.redesSociais)
      .subscribe({
        next:() => {
          this.toastr.success('Redes sociais salvas com sucesso!', 'Sucesso!');
        },
        error:(error: any) => {
          this.toastr.error('Erro ao tentar salvar redes sociais.', 'Erro');
          console.error(error);
        }
      }).add(()=> this.spinner.hide());
    }
  }

  public removerRedeSocial(template: TemplateRef<any>,
                      indice: number): void{

    this.redeSocialAtual.id =this.redesSociais.get(indice + '.id').value;
    this.redeSocialAtual.nome =this.redesSociais.get(indice + '.nome').value;
    this.redeSocialAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class:'modal-sm'})
  }

  confirmDeleteRedeSocial():void{
    let origem = 'palestrante';
    if(this.eventoId !== 0)
      origem = 'evento';

    this.modalRef.hide();
    this.spinner.show();

    this.redeSocialService.deleteRedeSocial(origem, this.eventoId, this.redeSocialAtual.id).subscribe({
      next: () => {
        this.toastr.success('Rede social deletada com sucesso', 'Sucesso');
        this.redesSociais.removeAt(this.redeSocialAtual.indice);
      },
      error: (error: any) => {
        this.toastr.error(`Erro ao tentar deletar a rede social ${this.redeSocialAtual.id}`, 'Erro');
      }
    }).add(()=> this.spinner.hide());
  }

  declineDeleteRedeSocial():void{
    this.modalRef.hide();
  }
}
