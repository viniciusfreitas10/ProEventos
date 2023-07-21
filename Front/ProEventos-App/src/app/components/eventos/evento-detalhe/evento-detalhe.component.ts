import { Template } from '@angular/compiler/src/render3/r3_ast';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
//import { error } from 'console';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { Toast, ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { Lote } from 'src/app/models/Lote';
import { EventoService } from 'src/app/services/Evento.service';
import { LoteService } from 'src/app/services/Lote.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

    modalRef: BsModalRef;
    eventoId: number;
    form!: FormGroup;
    evento = {} as Evento;
    ModeSave?: string = 'post';
    loteAtual = {id: 0, nome: "", indice: 0};
    imagemURL = `/assets/upload.png`;
    file: File;

    get f():any{
      return this.form.controls;
    }

    get modoEditar(): boolean{
      return this.ModeSave =='put'
    }
    get lotes(): FormArray{
      return this.form.get('lotes') as FormArray
    }
    get BsConfig():any{
      return {
        isAnimated: true,
        adaptivePosition: true,
        dateInputFormat: 'DD/MM/YYYY hh:mm a',
        //dateInputFormat: 'YYYY-MM-DD',
        containerClass: 'theme-default',
        showWeekNumbers: false
      };
    }

    get BsConfigLote():any{
      return {
        isAnimated: true,
        adaptivePosition: true,
        dateInputFormat: 'DD/MM/YYYY',
        containerClass: 'theme-default',
        showWeekNumbers: false
      };
    }

    get optionsFinanc():any{
      return { prefix: 'R$ ', thousands: '.', decimal: ',', align: 'left' };
    }

    constructor(private fb: FormBuilder,
      private localeService: BsLocaleService,
      private activatedRouter: ActivatedRoute,
      private eventoService: EventoService,
      private spiner: NgxSpinnerService,
      private toastr: ToastrService,
      private router: Router,
      private modalService: BsModalService,
      private loteservice: LoteService
      ) {
       this.localeService.use('pt-br');
      }

    ngOnInit(): void {
      this.validation();
      this.CarregarEvento();
    }

    public CarregarEvento(): void{
      this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

      if(this.eventoId !== null && this.eventoId != 0){
        this.ModeSave = 'put';

        this.eventoService.getEventoById(this.eventoId ).subscribe(
          (evento: Evento) => {
            this.evento = {...evento};
            this.form.patchValue(this.evento);
            if(this.evento.imagemURL !== ''){
              this.imagemURL = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
             }
            this.CarregarLotes(this.evento.lotes);
          },
          (error: any) => {
            console.log(error)
            this.spiner.hide();
            this.toastr.error("erro ao tentar carregar evento!")
          },
          () => {
            this.spiner.hide(); //ToDo: Refatorar
          }
        );
      }
    }

    public CarregarLotes(lotes: Lote[]): void{
      lotes.forEach(
        lote => {
          this.lotes.push(this.criarLote(lote));
        }
      )
    }

    public validation(): void{
      this.form = this.fb.group(
        {
          tema: ['',
          [Validators.required,Validators.maxLength(50), Validators.minLength(4)]
        ],
        local: ['',
        [Validators.required, Validators.maxLength(30), Validators.minLength(4)]
      ],
      dataEvento: ['', [Validators.required]],
      quantidadePessoas: ['',
      [Validators.required, Validators.max(120000), Validators.min(10)]
    ],
    imagemURL: [''],
    telefone: ['', [Validators.required, Validators.maxLength(16), Validators.minLength(11)]],
    email: ['', [Validators.required, Validators.email]],
    lotes: this.fb.array([]),
  });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio,Validators.required],
      dataFim: [lote.dataFim,Validators.required],
    });
  }


  public resetForm(): void{
    this.form.reset();
  }
  public cssValidator(campoForm: FormControl  | AbstractControl): any{
    return {'is-invalid': campoForm?.errors && campoForm?.touched}
  }

  public saveEvent(): void{
    this.spiner.show();
    if(this.form.valid){
      this.evento = (this.ModeSave == 'post') ? {...this.form.value} : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.ModeSave](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success("Evento atualizado com sucesso!", "Sucesso!");
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        (error: any) => {
          console.log(error);
          this.toastr.error("Erro ao atualizar o evento!", "Erro!")
          this.spiner.hide();
        },
        () => {this.spiner.hide();}
      )
    }
  }

  public saveLote(): void{
    if(this.form.controls.lotes.valid){
      this.spiner.show();
      this.loteservice.SaveLote(this.eventoId, this.form.value.lotes)
      .subscribe(
        () => {
          this.toastr.success("Lotes salvos com sucesso!", "Sucesso!");
        },
        (error: any) => {
          this.toastr.error("Erro ao tentar salvar os lotes", "Erro!")
          console.error(error);
          this.spiner.hide();
        }
      ).add(() => this.spiner.hide())
    };
  };

  public removerLote(template: TemplateRef<any>, indice: number): void
  {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'})
    //
  }

  public confirmarDeleteLote(): void{
    this.modalRef.hide();
    this.spiner.show();

    this.loteservice.DeleteLote(this.eventoId, this.loteAtual.id)
      .subscribe(
        () => {
          this.toastr.success("Lote deleteado com sucesso", "Sucesso")
          this.lotes.removeAt(this.loteAtual.indice);
        },
        (error: any) => {
          this.toastr.error(`Erro ao deletar o nome: ${this.loteAtual.nome}`);
          console.error(error);
          this.spiner.hide();
        }
      ).add(() => this.spiner.hide());
  }

  public declineDeleteLote(): void{
    this.modalRef.hide();
  };

  public returnTituloLote(nome: string): string{
    return nome === null || nome === '' ? 'Nome do Lote' : nome
  }

  public onFileChanges(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);
    this.uploadImage();
  }

  public uploadImage():void{
    this.spiner.show();
    this.eventoService.postUpload(this.eventoId, this.file)
      .subscribe(
        () => {
          this.CarregarEvento();
          this.toastr.success("Imagem atualizada com sucesso", "Sucesso!")
        },
        (error: any) => {
          this.toastr.error("Erro ao atualizar a imagem", "Erro!")
          console.error(error);
        }
      ).add(() => this.spiner.hide())
  }
}
