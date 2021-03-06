//Angular
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

//Componentes
import { AppComponent } from './app.component';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { PalestrantesComponent } from './componentes/palestrantes/palestrantes.component';
import { NavComponent } from './shared/nav/nav.component';
import { TituloComponent } from './shared/titulo/titulo.component';
import { ContatosComponent } from './componentes/contatos/contatos.component';
import { DashboardComponent } from './componentes/dashboard/dashboard.component';
import { PerfilComponent } from './componentes/user/perfil/perfil.component';
import { HomeComponent } from './componentes/home/home.component';

//ngx bootstrap
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';

//ngx
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxCurrencyModule } from 'ngx-currency';

//Serviços
import { EventoService } from './services/evento.service';
import { LoteService } from './services/lote.service';
import { AccountService } from './services/account.service';

//Pipes
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { EventoDetalheComponent } from './componentes/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './componentes/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './componentes/user/user.component';
import { LoginComponent } from './componentes/user/login/login.component';
import { RegistrationComponent } from './componentes/user/registration/registration.component';

defineLocale('pt-br', ptBrLocale);

import { CommonModule, registerLocaleData } from '@angular/common';
import localePT from '@angular/common/locales/pt';

import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { PerfilDetalheComponent } from './componentes/user/perfil/perfil-detalhe/perfil-detalhe.component';
import { PalestranteListaComponent } from './componentes/palestrantes/palestrante-lista/palestrante-lista.component';
import { PalestranteDetalheComponent } from './componentes/palestrantes/palestrante-detalhe/palestrante-detalhe.component';

import { PalestranteService } from './services/palestrante.service';
import { RedeSocialService } from './services/redeSocial.service';
import { RedesSociaisComponent } from './componentes/redesSociais/redesSociais.component';

registerLocaleData(localePT);

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
      PalestrantesComponent,
      PalestranteListaComponent,
      PalestranteDetalheComponent,
      ContatosComponent,
      DashboardComponent,
      PerfilComponent,
      PerfilDetalheComponent,
      RedesSociaisComponent,
      NavComponent,
      DateTimeFormatPipe,
      TituloComponent,
      EventoDetalheComponent,
      EventoListaComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      HomeComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule, // adicionar esse modulo para utilizar dentro do component.ts
                  //a referencia do HttpClient
    CollapseModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    CommonModule,
    NgxCurrencyModule,
    PaginationModule.forRoot(),
    TabsModule.forRoot()
  ],
  providers: [//terceira forma de declarar os arquivos que podem ser injetados (mais usada)
    EventoService,
    LoteService,
    AccountService,
    PalestranteService,
    RedeSocialService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
