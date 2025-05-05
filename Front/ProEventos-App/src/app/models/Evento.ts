import { User } from "./Identity/User";
import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
  id: number;
  local: string;
  dataEvento: Date;
  tema: string;
  quantidadePessoas: number;
  imagemURL: string;
  telefone: string;
  email: string;
  lotes: Lote[];
  redeSociai: RedeSocial[];
  palestrantesEventos: Palestrante[];
  userDto: User;
}
