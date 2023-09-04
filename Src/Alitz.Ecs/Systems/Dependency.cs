﻿using System;
using System.Collections.Generic;

namespace Alitz.Systems;
internal readonly record struct Dependency(Type SystemType, IReadOnlyList<Dependency> Dependencies);