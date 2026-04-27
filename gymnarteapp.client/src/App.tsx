import { useState } from 'react'

// ── Types ─────────────────────────────────────────────────────────────────────
interface Auth { token: string; userId: number; fullName: string; role: string }
interface Plan { id: number; name: string; type: string; exercises: number; notes: string }
interface BioEntry { date: string; weight: number; fat: number; lean: number; water: number; visceral: number }
interface Notif { id: number; text: string; read: boolean; time: string }

// ── Mock data ─────────────────────────────────────────────────────────────────
const PLANS: Plan[] = [
  { id: 1, name: 'Plano Força A', type: 'Musculação', exercises: 6, notes: 'Foco em pernas e costas' },
  { id: 2, name: 'Cardio Queima', type: 'Cardio', exercises: 4, notes: '30 min por sessão' },
]
const BIO_HISTORY: BioEntry[] = [
  { date: 'Mar 2025', weight: 82.5, fat: 18.2, lean: 72.1, water: 58.3, visceral: 9 },
  { date: 'Fev 2025', weight: 84.1, fat: 19.5, lean: 71.0, water: 57.8, visceral: 10 },
  { date: 'Jan 2025', weight: 85.8, fat: 21.0, lean: 69.5, water: 56.5, visceral: 11 },
  { date: 'Dez 2024', weight: 87.2, fat: 22.3, lean: 68.2, water: 55.9, visceral: 12 },
]
const NOTIFS: Notif[] = [
  { id: 1, text: 'Plano de treino atualizado pelo teu personal trainer', read: false, time: 'há 1 hora' },
  { id: 2, text: 'Novos dados biométricos registados', read: false, time: 'há 3 horas' },
  { id: 3, text: 'Parabéns! Completaste 10 treinos este mês 🎯', read: true, time: 'há 2 dias' },
  { id: 4, text: 'Bem-vindo ao GymnArte!', read: true, time: '12/01/2024' },
]
const EXERCISES_PLAN1 = [
  { name: 'Agachamento', sets: 4, reps: 12, notes: 'Progressão 2.5kg/semana' },
  { name: 'Supino Plano', sets: 4, reps: 10, notes: '' },
  { name: 'Remada Baixa', sets: 3, reps: 12, notes: 'Foco na retração escapular' },
  { name: 'Puxada Frontal', sets: 3, reps: 12, notes: '' },
  { name: 'Extensão de quadricípites', sets: 3, reps: 15, notes: '' },
  { name: 'Curl de bíceps', sets: 3, reps: 12, notes: '' },
]

// ── Icons ─────────────────────────────────────────────────────────────────────
const icons = {
  home: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>,
  dumbbell: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><path d="M6 5H4a2 2 0 00-2 2v10a2 2 0 002 2h2m12 0h2a2 2 0 002-2V7a2 2 0 00-2-2h-2M6 9H3M6 15H3M18 9h3M18 15h3M6 5v14M18 5v14M9 5h6v14H9z"/></svg>,
  chart: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><polyline points="22 12 18 12 15 21 9 3 6 12 2 12"/></svg>,
  bell: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><path d="M18 8A6 6 0 006 8c0 7-3 9-3 9h18s-3-2-3-9M13.73 21a2 2 0 01-3.46 0"/></svg>,
  user: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>,
  logout: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round"><path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4M16 17l5-5-5-5M21 12H9"/></svg>,
  chevron: <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><polyline points="9 18 15 12 9 6"/></svg>,
}

function initials(name: string) { return name.split(' ').slice(0, 2).map(p => p[0]).join('').toUpperCase() }

// ── Login ─────────────────────────────────────────────────────────────────────
function LoginPage({ onLogin }: { onLogin: (a: Auth) => void }) {
  const [email, setEmail] = useState('')
  const [pass, setPass] = useState('')
  const [err, setErr] = useState('')
  const [loading, setLoading] = useState(false)

  async function submit(e: React.FormEvent) {
    e.preventDefault(); setErr(''); setLoading(true)
    try {
      const res = await fetch('http://localhost:5297/api/auth/login', {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password: pass })
      })
      if (!res.ok) throw new Error()
      const data = await res.json()
      const auth = { token: data.token, userId: data.userId, fullName: data.fullName, role: data.role }
      localStorage.setItem('ga_client_auth', JSON.stringify(auth)); onLogin(auth)
    } catch {
      if (email === 'miguel@email.com' && pass === '1234') {
        const auth = { token: 'demo', userId: 1001, fullName: 'Miguel Costa', role: 'Member' }
        localStorage.setItem('ga_client_auth', JSON.stringify(auth)); onLogin(auth)
      } else setErr('Credenciais inválidas. Demo: miguel@email.com / 1234')
    }
    setLoading(false)
  }

  return (
    <div className="login-wrap">
      <div className="login-logo-wrap">
        <div className="login-logo">Gymn<span>Arte</span></div>
        <div className="login-tagline">O teu ginásio. O teu progresso.</div>
      </div>
      <div className="login-card">
        <form className="lf" onSubmit={submit}>
          <label>Email</label>
          <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="o.teu@email.com" required autoFocus />
          <label>Password</label>
          <input type="password" value={pass} onChange={e => setPass(e.target.value)} placeholder="••••••••" required />
          {err && <div className="lerr">{err}</div>}
          <div style={{ marginTop: 20 }}>
            <button className="btn btn-primary btn-full" disabled={loading} style={{ padding: 13, fontSize: 15 }}>
              {loading ? 'A entrar...' : 'Entrar'}
            </button>
          </div>
        </form>
      </div>
      <p style={{ fontSize: 11, color: '#bbb', marginTop: 16, textAlign: 'center' }}>© {new Date().getFullYear()} GymnArte</p>
    </div>
  )
}

// ── Dashboard ─────────────────────────────────────────────────────────────────
function Dashboard({ auth, onNav }: { auth: Auth; onNav: (p: string) => void }) {
  const bio = BIO_HISTORY[0]
  const hour = new Date().getHours()
  const greeting = hour < 12 ? 'Bom dia' : hour < 19 ? 'Boa tarde' : 'Boa noite'
  const unread = NOTIFS.filter(n => !n.read).length

  return (
    <>
      <div className="hdr">
        <div className="hdr-top">
          <div>
            <div className="hdr-logo">GymnArte</div>
          </div>
          <button className="notif-btn" onClick={() => onNav('notifs')}>
            {icons.bell}
            {unread > 0 && <div className="nbadge" />}
          </button>
        </div>
      </div>

      <div className="content" style={{ paddingBottom: 64 }}>
        {/* Hero */}
        <div className="hero-card">
          <div className="hero-greeting">{greeting} 👋</div>
          <div className="hero-name">{auth.fullName.split(' ')[0]}</div>
          <div className="hero-stats">
            <div><div className="hs-val">{PLANS.length}</div><div className="hs-lbl">Planos</div></div>
            <div><div className="hs-val">{bio.weight} kg</div><div className="hs-lbl">Último peso</div></div>
            <div><div className="hs-val">{bio.fat}%</div><div className="hs-lbl">Gordura</div></div>
          </div>
        </div>

        {/* Planos rápidos */}
        <div className="sec-t">Os teus planos de treino</div>
        <div className="card" style={{ marginLeft: 14, marginRight: 14 }}>
          {PLANS.map((p, i) => {
            const colors = ['#f97316','#6366f1','#10b981']
            return (
              <div key={p.id} className="row-item" onClick={() => onNav('training')}>
                <div className="ri-icon" style={{ background: colors[i % colors.length] + '20' }}>
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke={colors[i % colors.length]} strokeWidth="2"><path d="M6 5H4a2 2 0 00-2 2v10a2 2 0 002 2h2m12 0h2a2 2 0 002-2V7a2 2 0 00-2-2h-2M6 9H3M6 15H3M18 9h3M18 15h3M6 5v14M18 5v14M9 5h6v14H9z"/></svg>
                </div>
                <div className="ri-main">
                  <div className="ri-t">{p.name}</div>
                  <div className="ri-s">{p.type} · {p.exercises} exercícios</div>
                </div>
                <div style={{ color: '#ccc' }}>{icons.chevron}</div>
              </div>
            )
          })}
        </div>

        {/* Última biometria */}
        <div className="sec-t">Última medição</div>
        <div className="stats4" style={{ margin: '0 0 14px' }}>
          {[['Peso',`${bio.weight}`,'kg'],['% Gordura',`${bio.fat}`,'%'],['% M. Magra',`${bio.lean}`,'%'],['% Água',`${bio.water}`,'%']].map(([l,v,u]) => (
            <div key={l} className="stat-card" style={{ background: '#f8f8f6', borderRadius: 12, padding: 12 }}>
              <div className="stat-lbl">{l}</div>
              <div className="stat-val">{v}<span className="stat-unit"> {u}</span></div>
            </div>
          ))}
        </div>

        {/* Notificações recentes */}
        <div className="sec-t">Notificações</div>
        <div className="card" style={{ marginLeft: 14, marginRight: 14 }}>
          {NOTIFS.slice(0, 3).map(n => (
            <div key={n.id} className="notif-row">
              <div className={`notif-dot ${n.read ? 'nd-read' : 'nd-unread'}`} />
              <div style={{ flex: 1 }}>
                <div style={{ fontSize: 13 }}>{n.text}</div>
                <div style={{ fontSize: 11, color: '#888', marginTop: 2 }}>{n.time}</div>
              </div>
            </div>
          ))}
          <div style={{ padding: '10px 14px', textAlign: 'center' }}>
            <span className="card-link" onClick={() => onNav('notifs')} style={{ fontSize: 12 }}>Ver todas →</span>
          </div>
        </div>
      </div>
    </>
  )
}

// ── Training ─────────────────────────────────────────────────────────────────
function TrainingPage() {
  const [selPlan, setSelPlan] = useState<Plan | null>(null)
  const typeColors: Record<string, string> = { 'Musculação': '#6366f1', 'Cardio': '#f97316', 'Flexibilidade': '#10b981', 'Misto': '#8b5cf6' }

  if (selPlan) return (
    <>
      <div className="hdr">
        <div className="hdr-top">
          <button className="btn btn-ghost btn-sm" onClick={() => setSelPlan(null)}>← Voltar</button>
          <span className="badge" style={{ background: typeColors[selPlan.type] + '20', color: typeColors[selPlan.type] }}>{selPlan.type}</span>
        </div>
        <div style={{ marginTop: 8 }}>
          <div style={{ fontSize: 17, fontWeight: 700 }}>{selPlan.name}</div>
          {selPlan.notes && <div style={{ fontSize: 12, color: '#888', marginTop: 2 }}>{selPlan.notes}</div>}
        </div>
      </div>
      <div className="content">
        <div style={{ padding: '10px 0 0' }}>
          <div style={{ display: 'flex', gap: 10, padding: '0 14px 14px' }}>
            {[['Exercícios',selPlan.exercises],['Séries totais','18'],['Duração est.','55 min']].map(([l,v]) => (
              <div key={l as string} style={{ flex: 1, background: '#f8f8f6', borderRadius: 10, padding: '10px 12px', textAlign: 'center' }}>
                <div style={{ fontSize: 17, fontWeight: 700 }}>{v}</div>
                <div style={{ fontSize: 10, color: '#888', marginTop: 2 }}>{l}</div>
              </div>
            ))}
          </div>
          <div className="card" style={{ marginLeft: 14, marginRight: 14 }}>
            <div className="card-h"><span className="card-t">Exercícios</span></div>
            {EXERCISES_PLAN1.map((ex, i) => (
              <div key={i} className="ex-row">
                <div style={{ display: 'flex', alignItems: 'center', gap: 10, flex: 1 }}>
                  <div style={{ width: 28, height: 28, borderRadius: 8, background: typeColors[selPlan.type] + '20', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: 12, fontWeight: 700, color: typeColors[selPlan.type] }}>{i + 1}</div>
                  <div>
                    <div className="ex-name">{ex.name}</div>
                    <div className="ex-sets">{ex.sets} séries × {ex.reps} reps{ex.notes ? ` · ${ex.notes}` : ''}</div>
                  </div>
                </div>
                <div className="ex-pill">{ex.sets}×{ex.reps}</div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </>
  )

  return (
    <>
      <div className="hdr">
        <div className="hdr-top"><div style={{ fontSize: 17, fontWeight: 700 }}>Planos de treino</div></div>
      </div>
      <div className="content">
        {PLANS.map(p => (
          <div key={p.id} className="card" style={{ marginLeft: 14, marginRight: 14, marginTop: 14 }} onClick={() => setSelPlan(p)}>
            <div style={{ padding: '14px 14px 10px', display: 'flex', alignItems: 'center', gap: 12 }}>
              <div style={{ width: 44, height: 44, borderRadius: 12, background: typeColors[p.type] + '20', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke={typeColors[p.type]} strokeWidth="2"><path d="M6 5H4a2 2 0 00-2 2v10a2 2 0 002 2h2m12 0h2a2 2 0 002-2V7a2 2 0 00-2-2h-2M6 9H3M6 15H3M18 9h3M18 15h3M6 5v14M18 5v14M9 5h6v14H9z"/></svg>
              </div>
              <div style={{ flex: 1 }}>
                <div style={{ fontSize: 15, fontWeight: 600 }}>{p.name}</div>
                <div style={{ fontSize: 12, color: '#888', marginTop: 2 }}>{p.exercises} exercícios · {p.notes}</div>
              </div>
              <span className="badge" style={{ background: typeColors[p.type] + '20', color: typeColors[p.type] }}>{p.type}</span>
            </div>
            <div style={{ display: 'flex', borderTop: '1px solid #f2f2f0', padding: '8px 14px', gap: 16 }}>
              {['4 séries','Seg · Qua · Sex','~55 min'].map(s => (
                <span key={s} style={{ fontSize: 11, color: '#888' }}>{s}</span>
              ))}
            </div>
          </div>
        ))}
        <div style={{ padding: 14 }}>
          <button className="btn btn-ghost btn-full" style={{ padding: 12 }}>+ Pedir novo plano ao trainer</button>
        </div>
      </div>
    </>
  )
}

// ── Biometrics ────────────────────────────────────────────────────────────────
function BiometricsPage() {
  const latest = BIO_HISTORY[0]
  const maxW = Math.max(...BIO_HISTORY.map(b => b.weight))

  return (
    <>
      <div className="hdr">
        <div className="hdr-top"><div style={{ fontSize: 17, fontWeight: 700 }}>Dados biométricos</div></div>
      </div>
      <div className="content">
        {/* Peso ring */}
        <div className="card" style={{ margin: 14 }}>
          <div className="card-h"><span className="card-t">Peso atual</span><span style={{ fontSize: 11, color: '#888' }}>{latest.date}</span></div>
          <div style={{ display: 'flex', justifyContent: 'center', padding: '20px 0 14px', gap: 32 }}>
            {[['Peso',`${latest.weight} kg`],['IMC','25.4'],['Gordura visceral',`${latest.visceral}`]].map(([l,v]) => (
              <div key={l} style={{ textAlign: 'center' }}>
                <div style={{ fontSize: 22, fontWeight: 700, color: '#111' }}>{v}</div>
                <div style={{ fontSize: 10, color: '#888', marginTop: 3 }}>{l}</div>
              </div>
            ))}
          </div>
        </div>

        {/* Stats grid */}
        <div className="stats4" style={{ padding: '0 14px 14px', display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 10 }}>
          {[['% Gordura',latest.fat,'%','#f97316'],['% M. Magra',latest.lean,'%','#6366f1'],['% Água',latest.water,'%','#14b8a6'],['Gordura visceral',latest.visceral,'','#ec4899']].map(([l,v,u,c]) => (
            <div key={l as string} style={{ background: '#f8f8f6', borderRadius: 12, padding: 12, borderLeft: `3px solid ${c}` }}>
              <div style={{ fontSize: 10, color: '#888', marginBottom: 3 }}>{l}</div>
              <div style={{ fontSize: 22, fontWeight: 700 }}>{v}<span style={{ fontSize: 12, color: '#888', fontWeight: 400 }}> {u}</span></div>
            </div>
          ))}
        </div>

        {/* Weight chart */}
        <div className="card" style={{ margin: '0 14px 14px' }}>
          <div className="card-h"><span className="card-t">Evolução do peso</span></div>
          <div className="chart-bars">
            {[...BIO_HISTORY].reverse().map(b => (
              <div key={b.date} className="bar-col">
                <div className="bar-fill" style={{ height: `${((b.weight / maxW) * 60)}px` }} />
                <div className="bar-lbl">{b.date.slice(0, 3)}</div>
              </div>
            ))}
          </div>
        </div>

        {/* History table */}
        <div className="card" style={{ margin: '0 14px 14px' }}>
          <div className="card-h"><span className="card-t">Histórico completo</span></div>
          <div style={{ overflowX: 'auto' }}>
            <table style={{ fontSize: 12, width: '100%', borderCollapse: 'collapse' }}>
              <thead><tr style={{ borderBottom: '1px solid #eee' }}>
                {['Data','Peso','%Gordura','%M.Magra','%Água'].map(h => (
                  <th key={h} style={{ padding: '7px 12px', textAlign: 'left', fontSize: 10.5, fontWeight: 600, color: '#888', whiteSpace: 'nowrap' }}>{h}</th>
                ))}
              </tr></thead>
              <tbody>{BIO_HISTORY.map(b => (
                <tr key={b.date} style={{ borderBottom: '1px solid #f5f5f5' }}>
                  <td style={{ padding: '8px 12px', fontWeight: 500 }}>{b.date}</td>
                  <td style={{ padding: '8px 12px' }}>{b.weight} kg</td>
                  <td style={{ padding: '8px 12px' }}>{b.fat}%</td>
                  <td style={{ padding: '8px 12px' }}>{b.lean}%</td>
                  <td style={{ padding: '8px 12px' }}>{b.water}%</td>
                </tr>
              ))}</tbody>
            </table>
          </div>
        </div>
      </div>
    </>
  )
}

// ── Notifications ─────────────────────────────────────────────────────────────
function NotificationsPage() {
  const [notifs, setNotifs] = useState(NOTIFS)
  function markAll() { setNotifs(notifs.map(n => ({ ...n, read: true }))) }
  function mark(id: number) { setNotifs(notifs.map(n => n.id === id ? { ...n, read: true } : n)) }
  const unread = notifs.filter(n => !n.read).length

  return (
    <>
      <div className="hdr">
        <div className="hdr-top">
          <div style={{ fontSize: 17, fontWeight: 700 }}>Notificações</div>
          {unread > 0 && <button className="btn btn-ghost btn-sm" onClick={markAll}>Marcar todas</button>}
        </div>
        {unread > 0 && <div style={{ fontSize: 12, color: '#888', marginTop: 4 }}>{unread} não lidas</div>}
      </div>
      <div className="content">
        <div className="card" style={{ margin: 14 }}>
          {notifs.map(n => (
            <div key={n.id} className="notif-row">
              <div className={`notif-dot ${n.read ? 'nd-read' : 'nd-unread'}`} />
              <div style={{ flex: 1 }}>
                <div style={{ fontSize: 13, fontWeight: n.read ? 400 : 500 }}>{n.text}</div>
                <div style={{ fontSize: 11, color: '#888', marginTop: 3 }}>{n.time}</div>
              </div>
              {!n.read && <button className="btn btn-ghost btn-sm" style={{ fontSize: 10.5, padding: '4px 8px', flexShrink: 0 }} onClick={() => mark(n.id)}>Lida</button>}
            </div>
          ))}
        </div>
      </div>
    </>
  )
}

// ── Profile ───────────────────────────────────────────────────────────────────
function ProfilePage({ auth, onLogout }: { auth: Auth; onLogout: () => void }) {
  const ini = initials(auth.fullName)
  const items = [
    { icon: '👤', label: 'Dados pessoais', sub: 'Nome, email, telefone' },
    { icon: '🔒', label: 'Segurança', sub: 'Alterar password' },
    { icon: '🔔', label: 'Notificações', sub: 'Preferências de notificação' },
    { icon: '📊', label: 'Exportar dados', sub: 'Download do histórico' },
    { icon: '❓', label: 'Ajuda', sub: 'FAQ e suporte' },
  ]

  return (
    <>
      <div className="prof-header">
        <div className="prof-av">{ini}</div>
        <div className="prof-name">{auth.fullName}</div>
        <div className="prof-sub">Sócio #{1001} · {auth.role}</div>
        <div className="prof-stats">
          <div><div className="ps-val">2</div><div className="ps-lbl">Planos</div></div>
          <div><div className="ps-val">4</div><div className="ps-lbl">Medições</div></div>
          <div><div className="ps-val">12</div><div className="ps-lbl">Treinos</div></div>
        </div>
      </div>
      <div className="content" style={{ paddingBottom: 80 }}>
        <div className="card" style={{ margin: 14 }}>
          {items.map(item => (
            <div key={item.label} className="row-item">
              <div style={{ fontSize: 20 }}>{item.icon}</div>
              <div className="ri-main">
                <div className="ri-t">{item.label}</div>
                <div className="ri-s">{item.sub}</div>
              </div>
              <div style={{ color: '#ccc' }}>{icons.chevron}</div>
            </div>
          ))}
        </div>
        <div style={{ padding: '0 14px' }}>
          <button className="btn btn-ghost btn-full" onClick={onLogout} style={{ padding: 12, color: '#dc2626', borderColor: '#fca5a5' }}>
            {icons.logout} Terminar sessão
          </button>
        </div>
      </div>
    </>
  )
}

// ── Shell ─────────────────────────────────────────────────────────────────────
function Shell({ auth, onLogout }: { auth: Auth; onLogout: () => void }) {
  const [page, setPage] = useState('home')

  const nav = [
    { id: 'home', label: 'Início', icon: icons.home },
    { id: 'training', label: 'Treino', icon: icons.dumbbell },
    { id: 'biometrics', label: 'Biometria', icon: icons.chart },
    { id: 'notifs', label: 'Notificações', icon: icons.bell },
    { id: 'profile', label: 'Perfil', icon: icons.user },
  ]

  return (
    <div className="app-shell">
      {page === 'home' && <Dashboard auth={auth} onNav={setPage} />}
      {page === 'training' && <TrainingPage />}
      {page === 'biometrics' && <BiometricsPage />}
      {page === 'notifs' && <NotificationsPage />}
      {page === 'profile' && <ProfilePage auth={auth} onLogout={onLogout} />}

      <nav className="bnav">
        {nav.map(n => (
          <div key={n.id} className={`bni${page === n.id ? ' on' : ''}`} onClick={() => setPage(n.id)}>
            {n.icon}
            <span>{n.label}</span>
          </div>
        ))}
      </nav>
    </div>
  )
}

// ── App ───────────────────────────────────────────────────────────────────────
export default function App() {
  const [auth, setAuth] = useState<Auth | null>(() => {
    try { const s = localStorage.getItem('ga_client_auth'); return s ? JSON.parse(s) : null } catch { return null }
  })
  function logout() { localStorage.removeItem('ga_client_auth'); setAuth(null) }
  if (!auth) return <LoginPage onLogin={setAuth} />
  return <Shell auth={auth} onLogout={logout} />
}
