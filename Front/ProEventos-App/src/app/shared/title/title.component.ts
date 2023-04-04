import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss']
})
export class TitleComponent implements OnInit {

  @Input() titulo:string = '';
  @Input() subtitle: string = 'desde 2023';
  @Input() iconClass: string = 'fa fa-user';
  @Input() botaoListar: boolean = false;
  routeButtonList!: string;
  constructor(private router: Router) { }

  ngOnInit() {
  }

  listar(): void{
    this.routeButtonList = this.titulo.toLocaleLowerCase();

    this.router.navigate([`/${this.routeButtonList}/lista`]);
  }
}
