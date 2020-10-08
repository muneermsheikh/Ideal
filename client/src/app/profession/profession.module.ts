import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfessionComponent } from './profession.component';
import { SharedModule } from '../shared/shared.module';
import { ProfessionRoutingModule } from './profession-routing.module';
import { ProfessionDetailComponent } from './profession-detail/profession-detail.component';
import { ProfessionAddComponent } from './profession-add/profession-add.component';


@NgModule({
  declarations: [ProfessionComponent, ProfessionDetailComponent, ProfessionAddComponent],
  imports: [
    CommonModule,
    SharedModule,
    ProfessionRoutingModule
  ],
  exports: [
    // ProfessionComponent
  ]
})
export class ProfessionModule { }
