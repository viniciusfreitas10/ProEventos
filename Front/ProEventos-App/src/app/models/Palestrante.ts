import { RedeSocial } from "./RedeSocial";

export interface Palestrante {
  id: number;
  nome: string;
  miniCurriculo: string;
  imagemUrl: string;
  telefone: string;
  email: string;
  redeSociais: RedeSocial[];
  palestrantesEventos: Palestrante[];
}
