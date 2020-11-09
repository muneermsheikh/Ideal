import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { EmployeesComponent } from './employees/employees.component';
import { EmpCreateComponent } from './emp-create/emp-create.component';

const routes: Routes = [
  {path: 'employees', component: EmployeesComponent},
  {path: 'empCreate', component: EmpCreateComponent},
  {path: ':id', component: EmployeesComponent}
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
export class EmployeesRoutingModule { }
