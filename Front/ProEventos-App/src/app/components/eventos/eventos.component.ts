import { Component, OnInit, TemplateRef } from '@angular/core';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/Evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrado:Evento[] = [];
  public ImageWidth:number = 100;
  public ImageMargin: number = 10;
  public message:string = "";
  isCollapsed = true;
  ExibirImagem: boolean = true;
  private _filtroListaPaginaInicial: string = '';

  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ) { }

  ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
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
    this.eventoService.getEventos().subscribe({
     next: (_eventos:Evento[])  => {
        this.eventos = _eventos,
        this.eventosFiltrado = this.eventos
        console.log(_eventos)
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro!')
      },
      complete: () => this.spinner.hide(),
    });
  }
  openModal(template: TemplateRef<any>): void{
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O Evento foi deletado com sucesso','Deletado!');
  }

  decline(): void {
    this.modalRef?.hide();
  }
}
