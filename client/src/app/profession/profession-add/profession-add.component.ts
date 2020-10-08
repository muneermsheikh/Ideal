import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { ISkillLevel } from 'src/app/shared/models/skillLevel';
import { ProfessionService } from '../profession.service';

@Component({
  selector: 'app-profession-add',
  templateUrl: './profession-add.component.html',
  styleUrls: ['./profession-add.component.scss']
})
export class ProfessionAddComponent implements OnInit {

  addProfForm: FormGroup;
  indTypes: IIndustryType[];
  industryTypeId: number;
  skillLevelId: number;
  skLevels: ISkillLevel[];
  errors: string[];

  constructor(private fb: FormBuilder, private profService: ProfessionService, private router: Router) { }

  ngOnInit(): void {
    this.createAddForm();
    this.getIndustryTypes();
    this.getSkillLevels();
  }

  getIndustryTypes(): any {
    this.profService.getIndustryTypes().subscribe(response =>
      {this.indTypes = response; },
      error => {console.log(error); } );
  }

  getSkillLevels(): any {
    this.profService.getSkillLevels().subscribe(response =>
      {this.skLevels = response; },
      error => {console.log(error); } );
  }

createAddForm() {
    this.addProfForm = this.fb.group({
      name: [null, [Validators.required]],
      industryTypeId: [null, [Validators.required]],
      skillLevelId: [null, [Validators.required]]
    });
  }
  /*
onSumit() {
    console.log(this.editForm.value);
    this.profService.updateProfession(this.editForm.value).subscribe(response => {
      this.router.navigateByUrl('/profession');
     }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

  */

  onSubmit() {
    console.log(this.addProfForm.value);
    this.profService.addProfession(JSON.stringify(
        this.addProfForm.value,
        function replacer(key, value){
          if (key === 'industryTypeId' || key === 'skillLevelId') {
            return Number(value);
          }
        })).subscribe(response => {
      this.router.navigateByUrl('/profession');
     }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

}
