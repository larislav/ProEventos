import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { DashboardComponent } from './componentes/dashboard/dashboard.component';
import { ContatosComponent } from './componentes/contatos/contatos.component';
import { PerfilComponent } from './componentes/perfil/perfil.component';
import { PalestrantesComponent } from './componentes/palestrantes/palestrantes.component';

const routes: Routes = [
  {path: 'eventos', component: EventosComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'contatos', component: ContatosComponent},
  {path: 'perfil', component: PerfilComponent},
  {path: 'palestrantes', component: PalestrantesComponent},
  {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
  {path: '**', redirectTo: 'dashboard', pathMatch: 'full'} // default se for digitada rota que não está mapeada
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
