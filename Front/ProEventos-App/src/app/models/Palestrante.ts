import { Evento } from "./Evento";
import { UserUpdate } from "./identity/UserUpdate";
import { RedeSocial } from "./RedeSocial";

export interface Palestrante {

  id: number;
  nome: string;
  minicurriculo: string;
  user: UserUpdate;
  redesSociais: RedeSocial[];
  palestranteseventos: Evento[];
}
