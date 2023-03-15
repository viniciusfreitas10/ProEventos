import { Component, OnInit } from '@angular/core';
import { Evento } from '../models/Evento';
import { EventoService } from '../services/Evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventosFiltrado:Evento[] = [];
  public ImageWidth:number = 100;
  public ImageMargin: number = 10;
  isCollapsed = true;
  ExibirImagem: boolean = true;
  private _filtroListaPaginaInicial: string = '';

  constructor(private eventoService: EventoService) { }

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
  public FiltrarEventos(filtrarPor: string): Evento[]{
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
    this.eventoService.getEventos().subscribe(
    (_eventos:Evento[])  => {
        this.eventos = _eventos,
        this.eventosFiltrado = this.eventos
        console.log(_eventos)
      },
      error => console.log(error),
    );
  }

}
