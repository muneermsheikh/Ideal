import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateComponent } from './candidate/candidate.component';
import { ClientsComponent } from './clients/clients.component';
import { AssociatesComponent } from './associates/associates.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CandidateCreateComponent } from './candidate/candidate-create/candidate-create.component';
import { ClientCreateComponent } from './clients/client-create/client-create.component';


@NgModule({
  declarations: [CandidateComponent, ClientsComponent, AssociatesComponent,
    CandidateCreateComponent,
    ClientCreateComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UsersRoutingModule
  ],
  exports: [
    AssociatesComponent,
    CandidateComponent,
    ClientsComponent,
    SharedModule
  ]
})
export class UsersModule { }
