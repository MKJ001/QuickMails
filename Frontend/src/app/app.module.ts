import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NewGroupComponent } from './new-group/new-group.component';
import { HttpClientModule } from '@angular/common/http';
import { BackendService } from './services/backend.service';
import { InlineEmailInputComponent } from './controls/inline-email-input.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { MailToService } from './services/mailto.service';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    ModalModule.forRoot(),
    TooltipModule.forRoot()
  ],
  declarations: [
    AppComponent,
    NewGroupComponent,
    InlineEmailInputComponent
  ],
  providers: [
    BackendService,
    MailToService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
