import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateComponent } from './candidate/candidate.component';
import { ClientsComponent } from './clients/clients.component';
import { AssociatesComponent } from './associates/associates.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ClientAddComponent } from './clients/client-add/client-add.component';
import { ClientDetailComponent } from './clients/client-detail/client-detail.component';
import { CandidateCreateComponent } from './candidate/candidate-create/candidate-create.component';


@NgModule({
  declarations: [CandidateComponent, ClientsComponent, AssociatesComponent,
    ClientAddComponent,
    ClientDetailComponent, CandidateCreateComponent],
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
