import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IProfession } from 'src/app/shared/models/profession';
import { ISkillLevel } from 'src/app/shared/models/skillLevel';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ProfessionService } from '../profession.service';

@Component({
  selector: 'app-profession-detail',
  templateUrl: './profession-detail.component.html',
  styleUrls: ['./profession-detail.component.scss']
})
export class ProfessionDetailComponent implements OnInit {
  profession: IProfession;
  editForm: FormGroup;
  indTypes: IIndustryType[];
  industryTypeId: number;
  skillLevelId: number;
  skLevels: ISkillLevel[];
  errors: string[];

  constructor(private fb: FormBuilder, private profService: ProfessionService, private activatedRoute: ActivatedRoute,
              private router: Router, private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadCategory();
    this.getIndustryTypes();
    this.getSkillLevels();
    this.createEditForm();
  }

  loadCategory(): void {
    console.log('entered load category');
    this.profService.getProfession(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe( profession => {
      this.profession = profession;
      this.bcService.set('@categoryname', profession.name);
      console.log('finishedcall from profService.getprofession');
      console.log(this.profession);
    }, error => {
      console.log(error);
    });
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

  createEditForm() {
    this.editForm = this.fb.group({
      name: [null, [Validators.required]],
      industryTypeId: [null, [Validators.required]],
      skillLevelId: [null, [Validators.required]]
    });
  }

  onSubmit() {
    console.log(this.editForm.value);
    this.profService.updateProfession(this.editForm.value).subscribe(response => {
      this.router.navigateByUrl('/profession');
     }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

}
