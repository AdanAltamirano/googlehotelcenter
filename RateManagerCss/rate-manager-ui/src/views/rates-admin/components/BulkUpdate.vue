<template>
    <div id="rates" class="collapse pl-5 pr-5 pt-3">
                <div class="first-container">
                    <h3>Definición de Tarifas <span class="h5">Habitación D2Q - Habitación Doble</span></h3>
                </div>
                <div class="formulario-page overflow-auto pb-5">
                    <div class="bg-light">
                        <div class="rate-plan d-flex justify-content-between border-top mt-2 mb-5 pl-3 pt-3 pr-3">
                            <div class="w-25 pr-3">
                                <label>Plan Tarifario:</label>
                                <div class="form-group">
                                    <select class="form-control text-dark" id="ratePlan" name="kids">
                                        <option>PRUE -- PRUEBA PASARELA</option>
                                        <option>PRUE 1</option>
                                        <option>PRUE 2</option>
                                        <option>PRUE 3</option>
                                        <option>PRUE 4</option>
                                    </select>
                                </div>
                            </div>
                            <div class="d-flex justify-content-start w-75">
                                <div>
                                    <label>Desde:</label>
                                    <div class="input-group">
                                        <input type="date" class="border rounded-left text-secondary p-1" id="check-in"
                                            name="check-in" placeholder="dd/mm/yyyy">
                                        <div class="input-group-append">
                                            <i class="fa fa-calendar input-group-text pt-2" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                                <div class="ml-3">
                                    <label>Hasta:</label>
                                    <div class="input-group">
                                        <input type="date" class="border rounded-left text-secondary p-1" id="check-in"
                                            name="check-in" placeholder="dd/mm/yyyy">
                                        <div class="input-group-append">
                                            <i class="fa fa-calendar input-group-text pt-2" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="collapse-containers promotion-container mt-3">
                            <div class="d-flex justify-content-between align-items-center border dark-gray-created" id="headingOne">
                                <button class="btn shadow-none d-block w-100 p-0" type="button" data-toggle="collapse"
                                    data-target="#promotion" aria-expanded="false" aria-controls="collapseExample">
                                    <div class="d-flex justify-content-between align-items-center pl-3 pr-3 pt-2 pb-2">
                                        <label class="font-weight-bold text-left w-25 mb-0">Promociones:</label>
                                        <label class="font-weight-bold text-left w-50 mb-0">Ventana de Reserva:</label>
                                        <label class="font-weight-bold text-left w-25 mb-0">Ocupación por Habitación</label>
                                        <i class="fa fa-minus" aria-hidden="true"></i>
                                    </div>
                                </button>
                            </div>
                            <div class="collapse show" id="promotion">
                                <div class="d-flex justify-content-between p-3">
                                    <div class="first-column w-25 pr-3">
                                        <div class="d-flex justify-content-start">
                                            <p>Promoción de:</p>
                                            <input type="text" class="form-control w-25 ml-3 mr-3">
                                            <p>%</p>
                                        </div>
                                        <div class="mt-3">
                                            <p>Descripción de la promoción:</p>
                                            <div class="btn-group btn-group-toggle d-flex w-100" data-toggle="buttons">
                                                <label class="btn btn-primary shadow-none w-50">
                                                    <input type="radio" name="options" id="option1" autocomplete="off" checked>
                                                    Inglés
                                                </label>
                                                <label class="btn btn-primary active shadow-none w-50">
                                                    <input type="radio" name="options" id="option2" autocomplete="off"> Español
                                                </label>
                                            </div>
                                            <input type="text" class="form-control w-100 mt-2">
                                        </div>
                                    </div>
                                    <div class="second-column">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" value="" id="defaultCheck1">
                                            <label class="form-check-label" for="defaultCheck1">
                                                Ventana de Reserva
                                            </label>
                                        </div>
                                        <div class="mt-3">
                                            <div>
                                                <label>Desde:</label>
                                                <div class="input-group">
                                                    <input type="date" class="border rounded-left text-secondary p-1" id="check-in"
                                                        name="check-in" placeholder="dd/mm/yyyy">
                                                    <div class="input-group-append">
                                                        <i class="fa fa-calendar input-group-text pt-2" aria-hidden="true"></i>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="mt-3">
                                                <label>Hasta:</label>
                                                <div class="input-group">
                                                    <input type="date" class="border rounded-left text-secondary p-1" id="check-in"
                                                        name="check-in" placeholder="dd/mm/yyyy">
                                                    <div class="input-group-append">
                                                        <i class="fa fa-calendar input-group-text pt-2" aria-hidden="true"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="third-column w-40">
                                        <div class="d-flex justify-content-between">
                                            <div class="w-50">
                                                <div class="d-flex justify-content-between mb-3">
                                                    <label class="mb-0 mr-1">Max. Personas</label>
                                                    <div class="form-group w-40">
                                                        <select class="form-control" id="maxPersons" name="max-persons">
                                                            <option></option>
                                                            <option>1</option>
                                                            <option>2</option>
                                                            <option>3</option>
                                                            <option>4</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="d-flex justify-content-between mb-3">
                                                    <label class="mb-0 mr-1">Min. de Adultos</label>
                                                    <div class="form-group w-40">
                                                        <select class="form-control" id="minAdults" name="min-adults">
                                                            <option></option>
                                                            <option>1</option>
                                                            <option>2</option>
                                                            <option>3</option>
                                                            <option>4</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="d-flex justify-content-between">
                                                    <label class="mb-0 mr-1">Personas Extras</label>
                                                    <div class="form-group w-40">
                                                        <select class="form-control" id="extraPersons" name="extra-persons">
                                                            <option></option>
                                                            <option>1</option>
                                                            <option>2</option>
                                                            <option>3</option>
                                                            <option>4</option>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="w-50 ml-3">
                                                <div class="d-flex justify-content-between mb-3">
                                                    <label class="mb-0 mr-1">Niños</label>
                                                    <div class="form-group w-40">
                                                        <select class="form-control" id="kids" name="kids">
                                                            <option></option>
                                                            <option>1</option>
                                                            <option>2</option>
                                                            <option>3</option>
                                                            <option>4</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="d-flex justify-content-between mb-3">
                                                    <label class="mb-0 mr-1">Max. de Adultos</label>
                                                    <div class="form-group w-40">
                                                        <select class="form-control" id="maxAdults" name="max-adults">
                                                            <option></option>
                                                            <option>1</option>
                                                            <option>2</option>
                                                            <option>3</option>
                                                            <option>4</option>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="collapse-containers reservation-rules-container">
                            <div class="d-flex justify-content-between align-items-center border dark-gray-created" id="headingOne">
                                <button class="btn shadow-none d-block w-100 p-0" type="button" data-toggle="collapse"
                                    data-target="#reservation-rules" aria-expanded="false" aria-controls="collapseExample">
                                    <div class="d-flex justify-content-between align-items-center pl-3 pr-3 pt-2 pb-2">
                                        <label class="font-weight-bold mb-0">Reglas de Reservación:</label>
                                        <i class="fa fa-minus" aria-hidden="true"></i>
                                    </div>
                                </button>
                            </div>
                            <div class="collapse show" id="reservation-rules">
                                <div class="d-flex justify-content-between p-3">
                                    <div class="form-check w-30">
                                        <input class="form-check-input" type="checkbox" value="" id="defaultCheck1">
                                        <label class="form-check-label" for="defaultCheck1">
                                            Usar Reglas Default
                                        </label>
                                    </div>
                                    <div class="w-20">
                                        <p>Días avanzados para Reservar:</p>
                                        <div class="d-flex justify-content-between">
                                            <div class="d-flex justify-content-start">
                                                <p>Min:</p>
                                                <input type="text" class="form-control w-50 ml-3 mr-3">
                                            </div>
                                            <div class="d-flex justify-content-start">
                                                <p>Max:</p>
                                                <input type="text" class="form-control w-50 ml-3 mr-3">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="w-50 pl-5">
                                        <p>Ninguna Llegada:</p>
                                        <div class="d-flex justify-content-between mb-3">
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox1">Do</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox1"
                                                    value="option1">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Lu</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Ma</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Mi</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Ju</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Vi</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <label class="form-check-label mr-2" for="inlineCheckbox2">Sa</label>
                                                <input class="form-check-input" type="checkbox" id="inlineCheckbox2"
                                                    value="option2">
                                            </div>
                                        </div>
                                        <div class="d-flex justify-content-between">
                                            <div class="d-flex justify-content-start">
                                                <p>Min. Noches</p>
                                                <input type="text" class="form-control w-25 ml-3 mr-3">
                                            </div>
                                            <div class="d-flex justify-content-start">
                                                <p>Max. Noches</p>
                                                <input type="text" class="form-control w-25 ml-3 mr-3">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="gds-container">
                            <div class="border dark-gray-created pl-3 pr-3 pt-2 pb-2" id="headingOne">
                                <p class="font-weight-bold mb-0">Precios de la Habitación - Moneda <span class="text-primary">MXN</span> No incluye impuestos</p>
                            </div>
                            <div class="d-flex justify-content-between p-3">
                                <div class="form-check pl-0 w-30">
                                    <label class="font-weight-bold">Mostrar Tarifas por</label>
                                    <div class="custom-control custom-switch mb-3 pl-0">
                                        <label class="font-weight-bold mr-5">Habitación</label>
                                        <input type="checkbox" class="custom-control-input" id="customSwitches">
                                        <label class="custom-control-label label-style font-weight-bold" for="customSwitches">Ocupación</label>
                                        <!--label class="custom-control-label" for="customSwitches">Mostrar Tarifas por Ocupación</label>-->
                                    </div>
                                    <label>Adulto Extra:</label>
                                    <div class="d-flex justify-content-start">
                                        <input type="text" class="form-control w-50 mr-3">
                                        <p class="font-weight-bold"><span class="text-primary">MXN</span></p>
                                    </div>
                                    <label>Niño Extra:</label>
                                    <div class="d-flex justify-content-start">
                                        <input type="text" class="form-control w-50 mr-3">
                                        <p class="font-weight-bold"><span class="text-primary">MXN</span></p>
                                    </div>
                                    <label>Junior Extra:</label>
                                    <div class="d-flex justify-content-start">
                                        <input type="text" class="form-control w-50 mr-3">
                                        <p class="font-weight-bold"><span class="text-primary">MXN</span></p>
                                    </div>
                                </div>
                                <div class="form-check tab-container pl-0 w-70">
                                    <ul class="nav nav-tabs nav-justified">
                                        <li class="nav-item">
                                            <a class="nav-link active border text-dark" data-toggle="tab"
                                                href="#priceRates">[Precios]</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link border text-dark" data-toggle="tab"
                                                href="#priceExceptions">[Excepciones en Precios]</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link text-dark invisible" data-toggle="tab" href="#menu2"></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content">
                                        <div id="priceRates" class="tab-pane active">
                                            <div class="form-check d-flex justify-content-between  pl-0">
                                                <div class="price-rates">
                                                    <div class="d-flex justify-content-between p-2">
                                                        <label>Adultos</label>
                                                        <label>Tarifa de Adultos</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">1</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">2</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">3</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                </div>
                                                <div class="price-rates price-rates-selected">
                                                    <div class="d-flex justify-content-between p-2">
                                                        <label>Niños</label>
                                                        <label>Tarifa de Niños</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">1</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">2</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">3</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                </div>
                                                <div class="junior-rates">
                                                    <div class="d-flex justify-content-between p-2">
                                                        <label>Junior</label>
                                                        <label>Tarifa de Junior</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">1</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-20">2</label>
                                                        <input type="text" class="form-control text-right w-60 ml-3 mr-3" value="1">
                                                        <label class="font-weight-bold text-primary w-20">MXN</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="priceExceptions" class="container tab-pane fade"><br>
                                            <h3>Menu 1</h3>
                                            <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex
                                                ea commodo
                                                consequat.</p>
                                        </div>
                                        <div id="menu2" class="container tab-pane fade"><br>
                                            <h3>Menu 2</h3>
                                            <p>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque
                                                laudantium, totam
                                                rem aperiam.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="p-3 mt-3">
                                <button type="button" class="btn text-primary m-2"><i class="fa fa-undo mr-3"></i>Reset</button>
                                <button type="button" class="btn btn-success m-2">Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
</template>
