import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateComponent } from './candidate/candidate.component';
import { ClientsComponent } from './clients/clients.component';
import { AssociatesComponent } from './associates/associates.component';
import { EmployeesComponent } from './employees/employees.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CandidateAddComponent } from './candidate/candidate-add/candidate-add.component';
import { CandidateEditComponent } from './candidate/candidate-edit/candidate-edit.component';
import { ClientAddComponent } from './clients/client-add/client-add.component';
import { ClientDetailComponent } from './clients/client-detail/client-detail.component';
import { CandidateCreateComponent } from './candidate/candidate-create/candidate-create.component';


@NgModule({
  declarations: [CandidateComponent, ClientsComponent, AssociatesComponent,
    EmployeesComponent, CandidateAddComponent, CandidateEditComponent, ClientAddComponent, ClientDetailComponent, CandidateCreateComponent],
  imports: [
    CommonModule,
    SharedModule,
    UsersRoutingModule
  ],
  exports: [
    AssociatesComponent,
    CandidateComponent,
    ClientsComponent,
    EmployeesComponent,
    SharedModule
  ]
})
export class UsersModule { }
