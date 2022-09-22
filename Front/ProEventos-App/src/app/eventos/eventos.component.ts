import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrado:any = [];
  public ImageWidth = 100;
  public ImageMargin = 10;
  isCollapsed = true;
  ExibirImagem: boolean = true;
  private _filtroListaPaginaInicial: string = '';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }
  public get filtroListaPaginaInicial(): string{
    return this._filtroListaPaginaInicial;
  }
  public set filtroListaPaginaInicial(NewFiltroListaPaginaInicial: string ){
    this._filtroListaPaginaInicial = NewFiltroListaPaginaInicial;
    this.eventosFiltrado = this.filtroListaPaginaInicial ? this.FiltrarEventos(this.filtroListaPaginaInicial) : this.eventos;
  }
  public FiltrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleUpperCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleUpperCase().indexOf(filtrarPor) !== -1 ||
       evento.local.toLocaleUpperCase().indexOf(filtrarPor) !== - 1 || evento.dataEvento.toLocaleUpperCase().indexOf(filtrarPor) !== -1
    )
  }
  public controleDeExibicaoDeImagem(): void{
    this.ExibirImagem = !this.ExibirImagem;
  }
  public getEventos(): void{
    this.http.get("https://localhost:5001/api/Evento/").subscribe(
      response => {
        this.eventos = response,
        this.eventosFiltrado = response
      },
      error => console.log(error),
    );
  }
}
