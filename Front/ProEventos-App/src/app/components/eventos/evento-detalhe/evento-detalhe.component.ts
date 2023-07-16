import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { Toast, ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { Lote } from 'src/app/models/Lote';
import { EventoService } from 'src/app/services/Evento.service';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

    form!: FormGroup;
    evento = {} as Evento;
    ModeSave?: string = 'post';

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

    constructor(private fb: FormBuilder,
      private localeService: BsLocaleService,
      private activatedRouter: ActivatedRoute,
      private eventoService: EventoService,
      private spiner: NgxSpinnerService,
      private toastr: ToastrService,
      private router: Router
      ) {
       this.localeService.use('pt-br');
      }

    ngOnInit(): void {
      this.validation();
      this.CarregarEvento();
    }

    public CarregarEvento(): void{
      var eventoIdaram = this.activatedRouter.snapshot.paramMap.get('id');

      if(eventoIdaram !== null){
        this.ModeSave = 'put';

        this.eventoService.getEventoById(+eventoIdaram).subscribe(
          (evento: Evento) => {
            this.evento = {...evento};
            this.form.patchValue(this.evento);
          },
          (error: any) => {
            console.log(error)
            this.spiner.hide();
            this.toastr.error("erro ao tentar carregar evento!")
          },
          () => {
            this.spiner.hide();
          }
        );
      }
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
    imagemURL: ['', [Validators.required, Validators.maxLength(30), Validators.minLength(3)]],
    telefone: ['', [Validators.required, Validators.maxLength(16), Validators.minLength(11)]],
    email: ['', [Validators.required, Validators.maxLength(30), Validators.minLength(4), Validators.email]],
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

  public ConfirmAlterationEvent(): void{
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

}
