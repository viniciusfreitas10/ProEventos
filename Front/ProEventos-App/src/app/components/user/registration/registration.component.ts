import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { ValidatorFields } from 'src/app/helpers/ValidatorFields';
import { User } from 'src/app/models/Identity/User';
import { AccountService } from 'src/app/services/Account.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User;
  form!: FormGroup;

  get f():any{
    return this.form.controls;
  };

  constructor(private fb: FormBuilder,
              private accontService: AccountService,
              private router: Router,
              private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.validation()
  }

  public validation(): void{

    const formOptions: AbstractControlOptions ={
      validators: ValidatorFields.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group(
      {
        primeiroNome:['', [Validators.required]] ,
        ultimoNome:['', [Validators.required]] ,
        email:['', [Validators.required, Validators.maxLength(30), Validators.minLength(4), Validators.email]] ,
        userName:['', [Validators.required]] ,
        password:['', [Validators.required, Validators.minLength(6)]] ,
        confirmePassword:['', [Validators.required, Validators.minLength(6)]]
      }, formOptions
  );

}

  public Register():void{
    this.user = {...this.form.value};
    this.accontService.Register(this.user).subscribe(
      () =>{
        this.router.navigateByUrl('/dashboard'),
      (error: any) => {
        this.toaster.error("Erro ao registrar o usuário! Entre em contato com o Administrador!"),
        console.log("Erro ao cadastrar: " + error)
      },
      () => {
        this.toaster.success("Usuário cadastrado com sucesso!");
      }
      }
    )
  }

}
