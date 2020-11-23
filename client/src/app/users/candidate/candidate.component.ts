import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CandidateParams } from 'src/app/shared/models/CandidateParams';
import { ICandidate } from 'src/app/shared/models/ICand';
import { ICategoryWithProf } from 'src/app/shared/models/ICategoryWithProf';
import { IProfession } from 'src/app/shared/models/profession';
import { UsersService } from '../users.service';

@Component({
  selector: 'app-candidate',
  templateUrl: './candidate.component.html',
  styleUrls: ['./candidate.component.scss']
})
export class CandidateComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  candidates: ICandidate[];
  professions: IProfession[];
  candProfessions: ICategoryWithProf[];
  form: FormGroup;
  errors: string[];

  candParams = new CandidateParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameTypeAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];

  constructor(private cvService: UsersService, private fb: FormBuilder,
              private router: Router) { }

  ngOnInit(): void {
    this.getProfessions();
    this.getCandidates();
  }

  editButtonClick(candidateId: number): void {
    this.router.navigate(['/candidateEdit', candidateId]);
  }

  getProfessions(): any {
    this.cvService.getProfessions().subscribe(response =>
      { this.professions = response; }
      , error => {
        console.log(error); });
  }

  getCandidateProfessions(): any {
    this.cvService.getCandCatsWithProf().subscribe(response => {
      this.candProfessions = response;
    }, error => {
      console.log(error);
    });
  }


  getCandidates(useCache = false): void {
      this.cvService.getCandidates(useCache).subscribe(response => {
      this.candidates = response.data;
      this.totalCount = response.count;
      this.candParams.pageNumber = response.pageIndex;
      this.candParams.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }
/*
  createForm(): void {
    this.form = this.fb.group({
      applicationNo: [null],
      applicationDated: [null],
      gender: [''],
      fullName: [''],
      // addedOn: [''],
      name: [null],
      categoryName: [null],
      passportNo: [null],
      aadharNo: [null],
      mobileNo: [null],
      email: [null]
    });
  }
*/
  onPageChanged(event: any): void {
    const params = this.cvService.getCandParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.cvService.setCandParams(params);
      this.getCandidates(true);
    }
  }
/*
  onPageChanged(event: any): void {
    const params = this.cvService.getCandParams();
    this.candParams.pageNumber = event;
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.cvService.setCandParams(params);
      this.getCandidates();
    }
  }
*/

  onSearch(): void{
    console.log(this.searchTerm.nativeElement.value);
    const params = this.cvService.getCandParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.cvService.setCandParams(params);

    this.getCandidates();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.candParams = new CandidateParams();
    // this.shopService.setShopParams(this.shopParams);
    this.getCandidates();
  }

  onSortSelected(sort: string): any {
    const params = this.cvService.getCandParams();
    params.sort = sort;
    this.cvService.setCandParams(params);
    this.getCandidates();
  }

}
