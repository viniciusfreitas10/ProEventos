import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFields } from 'src/app/helpers/ValidatorFields';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  form!: FormGroup;

  get f():any{
    return this.form.controls;
  };

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation()
  }

  public validation(): void{

    const formOptions: AbstractControlOptions ={
      validators: ValidatorFields.MustMatch('senha', 'confirmeSenha')
    };

    this.form = this.fb.group(
      {
        primeiroNome:['', [Validators.required]] ,
        ultimoNome:['', [Validators.required]] ,
        email:['', [Validators.required, Validators.maxLength(30), Validators.minLength(4), Validators.email]] ,
        userName:['', [Validators.required]] ,
        senha:['', [Validators.required, Validators.minLength(6)]] ,
        confirmeSenha:['', [Validators.required, Validators.minLength(6)]]
      }, formOptions
  );
}

}
