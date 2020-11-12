import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { City } from '../cities/city';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.css']
})
export class CityEditComponent implements OnInit {

  // this view title
  title: string;

  // the form model
  form: FormGroup;

  // the city object to edit or create
  city: City;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new city,
  // and not NULL when we're editing an existing one.
  id?: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl('')
    });

    this.loadData();
  }

  loadData() {
    // retrieve the ID form the 'id' parameter
    this.id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (this.id) {
      // EDIT MODE 

      // fetch the city form the server
      var url = this.baseUrl + "api/cities/" + this.id;
      this.http.get<City>(url).subscribe(result => {
        this.city = result;
        this.title = "Edit - " + this.city.name;

        // update the form with the city value
        this.form.patchValue(this.city);
      }, error => console.error(error));
    } else {
      // ADD NEW MODE
      this.title = "Create a new City";
    }
  }

  onSubmit() {
    var city = (this.id) ? this.city : <City>{};

    city.name = this.form.get("name").value;
    city.lat = this.form.get("lat").value;
    city.lon = this.form.get("lon").value;

    if (this.id) {
      // EDIT mode

      var url = this.baseUrl + "api/cities/" + this.city.id;
      this.http.put<City>(url, city).subscribe(result => {
        console.log("City " + city.id + " has been updated.");

        // go back to cities view
        this.router.navigate(['/cities']);
      }, error => console.log(error));
    } else {
      // ADD NEW mode
      var url = this.baseUrl + "api/cities";
      this.http.post<City>(url, city).subscribe(result => {
        
        console.log("City " + result.id + " has been created.");

        //go back to cities view
        this.router.navigate(['/cities']);
      }, error => console.log(error));
    }
  }

}
