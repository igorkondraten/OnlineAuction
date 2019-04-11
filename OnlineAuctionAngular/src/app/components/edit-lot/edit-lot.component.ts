import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from 'src/app/models/lot.model';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category.model';

@Component({
  selector: 'app-edit-lot',
  templateUrl: './edit-lot.component.html',
  styleUrls: ['./edit-lot.component.css']
})
export class EditLotComponent implements OnInit {
  lot: Lot;
  editLotForm: FormGroup;
  loading = false;
  submitted = false;
  categories: Category[] = [];
  image: any;

  get f() { return this.editLotForm.controls; }

  constructor(private route: ActivatedRoute,
    private lotService: LotService,
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder,
    private categoryService: CategoryService) { }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.lotService.get(+params['id']).subscribe(lot => {
        this.lot = lot;
        this.updateValues();
      });
    });
    this.categoryService.getAll().subscribe(
      categories => {
        this.categories = categories;
      }
    );
    this.editLotForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      initialPrice: ['', [Validators.required, Validators.min(0.01)]],
      date: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      category: ['', Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.editLotForm.invalid) {
      return;
    }
    this.loading = true;
    if (this.image)
      this.image = this.image.split("base64,")[1];
    this.lotService.update(this.lot.lotId, <Lot>{
      name: this.f.name.value,
      initialPrice: this.f.initialPrice.value,
      beginDate: this.f.date.value[0],
      endDate: this.f.date.value[1],
      description: this.f.description.value,
      category: <Category>{ categoryId: this.f.category.value },
      imageUrl: this.lot.imageUrl,
      image: this.image
    })
      .subscribe(
        () => {
          this.alertService.success('Lot updated');
          this.router.navigate(['lot', this.lot.lotId]);
        },
        error => {
          this.alertService.error(error.error.message);
          this.loading = false;
        });
  }

  updateValues() {
    this.editLotForm.setValue({
      name: this.lot.name,
      initialPrice: this.lot.initialPrice,
      date: [this.lot.beginDate, this.lot.endDate],
      description: this.lot.description,
      category: this.lot.category.categoryId
    });
  }

  imageUpload(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      this.alertService.error("Invalid image format.")
      return;
    }
    reader.onload = ((e) => {
      let reader = e.target;
      this.image = reader.result;
      console.log(this.image)
    }).bind(this);
    reader.readAsDataURL(file);
  }

}
