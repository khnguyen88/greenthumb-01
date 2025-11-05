import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardLightTrigger } from './dashboard-light-trigger';

describe('DashboardLightTrigger', () => {
  let component: DashboardLightTrigger;
  let fixture: ComponentFixture<DashboardLightTrigger>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardLightTrigger]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardLightTrigger);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
