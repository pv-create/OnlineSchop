#!/bin/bash

set -e

dotnet ef database update --project ./OnlineShop.ClientService
dotnet ef database update --project ./OnlineShop.OrderService