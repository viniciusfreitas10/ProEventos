import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;

  get f():any{
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
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
  console.log("cheguei no reset")
}

}
