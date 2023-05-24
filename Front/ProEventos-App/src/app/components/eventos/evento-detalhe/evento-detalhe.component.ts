import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { Toast, ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
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
      private router: ActivatedRoute,
      private eventoService: EventoService,
      private spiner: NgxSpinnerService,
      private toastr: ToastrService
      ) {
       this.localeService.use('pt-br');
      }

    ngOnInit(): void {
      this.validation();
      this.CarregarEvento();
    }

    public CarregarEvento(): void{
      var eventoIdaram = this.router.snapshot.paramMap.get('id');

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
    email: ['', [Validators.required, Validators.maxLength(30), Validators.minLength(4), Validators.email]]
  }
  );
  }

  public resetForm(): void{
    this.form.reset();
  }
  public cssValidator(campoForm: FormControl): any{
    return {'is-invalid': campoForm.errors && campoForm.touched}
  }

  public ConfirmAlterationEvent(): void{
    this.spiner.show();
    if(this.form.valid){
      this.evento = (this.ModeSave == 'post') ? {...this.form.value} : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.ModeSave](this.evento).subscribe(
        () => {
          this.toastr.success("Evento atualizado com sucesso!", "Sucesso!")
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
