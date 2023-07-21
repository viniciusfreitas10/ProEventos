import { Component, OnInit, TemplateRef } from '@angular/core';
import { Route, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/Evento.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrado:Evento[] = [];
  public ImageWidth:number = 100;
  public ImageMargin: number = 10;
  public message:string = "";
  isCollapsed = true;
  ExibirImagem: boolean = true;
  private _filtroListaPaginaInicial: string = '';
  public eventoId: number = 0;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
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

  public DeleteEvent(id: number): any{
    this.eventoService.DeleteEvento(id).subscribe(
      (result: any) =>{
        if(result.message === "Deletado"){
          this.toastr.success(`O Evento de id ${id} foi deletado com sucesso`,'Deletado!');
          this.getEventos();
        };
      },
      (error: any) => {
        console.log(error);
        this.toastr.error(`Erro ao excluir o evento de id ${id}!`, "Erro!");
      }
    ).add( () => this.spinner.hide());
  }

  openModal(event: any,template: TemplateRef<any>, _eventoId: number): void{
    this.eventoId = _eventoId;
    event.stopPropagation();
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();
    this.DeleteEvent(this.eventoId);
  }

  decline(): void {
    this.modalRef?.hide();
  }
  detalheEvento(id:number): void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL != '') ? `${environment.apiURL}resources/images/${imagemURL}` : '/assets/semImagem.png' ;
  }
}
