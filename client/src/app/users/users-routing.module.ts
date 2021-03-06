import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AssociatesComponent } from './associates/associates.component';
import { RouterModule, Routes } from '@angular/router';
import { ClientsComponent } from './clients/clients.component';
import { CandidateComponent } from './candidate/candidate.component';

const routes: Routes = [
  {path: 'associates', component: AssociatesComponent},
  {path: 'clients', component: ClientsComponent},
  {path: 'candidate', component: CandidateComponent}

];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class UsersRoutingModule { }
