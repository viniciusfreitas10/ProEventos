import { Component, OnInit } from '@angular/core';
import { UserLogin } from 'src/app/models/Identity/UserLogin';
import { AccountService } from '../../../services/Account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  model = {} as UserLogin;

  constructor(private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService
  ) { }

  ngOnInit(): void {
  }

  public Login(): void{
    this.accountService.login(this.model).subscribe(
      () => {
        this.router.navigateByUrl('/dashboard');
      },
      (error: any) => {
        if(error.status == 401){
          this.toaster.error("usuário ou senha inválidos")
        }else{
          this.toaster.error("Erro ao fazer login! Entre em contato com o administrador do sistema.")
          console.log("Erro login: " + error.message);
        }
      }
    )
  }

}
